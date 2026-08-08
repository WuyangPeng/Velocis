// 创建时间：2026-07-26
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.RuntimeException;
using GameFramework.Resource;
using Luban;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Config
{
    /// <summary>
    /// 游戏配置管理器，负责在运行时通过 GameFramework 的资源系统加载 Luban 二进制配置文件，并实例化配置表对象。
    /// </summary>
    public class GameConfig
    {
        private readonly Dictionary<string, ByteBuf> _byteBuf = new();
        private readonly Dictionary<System.Type, IConfigProcessor> _processors = new();
        private int _loadTablesSize;
        private tables _tables;
        private int _tablesSize;

        /// <summary>
        /// 获取 Luban 配置表实例。
        /// </summary>
        /// <returns>配置表实例对象。</returns>
        public tables GetTables()
        {
            return _tables;
        }

        /// <summary>
        /// 获取配置预处理器实例。
        /// </summary>
        public T GetProcessor<T>() where T : class, IConfigProcessor
        {
            if (_processors.TryGetValue(typeof(T), out var processor))
            {
                return processor as T;
            }
            return null;
        }

        /// <summary>
        /// 初始化配置加载，开始异步加载所有 Luban 配置表数据。
        /// </summary>
        public void Initialize()
        {
            _byteBuf.Clear();
            _loadTablesSize = 0;
            LoadByteBuf();
        }

        /// <summary>
        /// 异步加载所有配置表的二进制资产（TextAsset）。
        /// </summary>
        private void LoadByteBuf()
        {
            var tableNames = GetTableNames();
            _tablesSize = tableNames.Count;
            foreach (var tableName in tableNames)
            {
                GameEntry.Resource.LoadAsset(AssetUtility.GetLubanAsset(tableName),
                    typeof(TextAsset),
                    new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure));
            }
        }

        /// <summary>
        /// 通过反射获取 Luban tables 类中定义的所有配置表容器的名称（转换为蛇形命名）。
        /// </summary>
        /// <returns>配置表名称列表。</returns>
        private static List<string> GetTableNames()
        {
            var properties = typeof(tables).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return (from prop in properties where prop.Name.EndsWith("ConfigContainer") select ToSnakeCase(prop.Name)).ToList();
        }

        /// <summary>
        /// 将驼峰命名（PascalCase）转换为蛇形命名（snake_case）。
        /// </summary>
        /// <param name="str">待转换的字符串。</param>
        /// <returns>转换后的蛇形命名字符串。</returns>
        private static string ToSnakeCase(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var sb = new StringBuilder();
            sb.Append(char.ToLower(str[0]));
            for (var i = 1; i < str.Length; i++)
            {
                var c = str[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLower(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Luban 配置表加载委托实现，根据表名从已缓存的 ByteBuf 字典中提取对应的配置字节数据。
        /// </summary>
        /// <param name="file">配置表文件名（不含路径与后缀）。</param>
        /// <returns>Luban 反序列化所需的 ByteBuf 实例。</returns>
        private ByteBuf LoadByteBuf(string file)
        {
            return _byteBuf.TryGetValue(AssetUtility.GetLubanAsset(file), out var buf) ? buf : throw new GameException($"error byte,file = {file}");
        }

        /// <summary>
        /// 资源加载成功回调。
        /// </summary>
        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            var textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                Log.Error("Load asset is not text asset: {0}", assetName);
                return;
            }

            _byteBuf.Add(assetName, new ByteBuf(textAsset.bytes));
            Log.Info("load binary success,file = {0}", assetName);
            ++_loadTablesSize;
            if (_loadTablesSize != _tablesSize)
            {
                return;
            }

            _tables = new tables(LoadByteBuf);
            _byteBuf.Clear();
            InitializeProcessors();
        }

        /// <summary>
        /// 自动扫描并初始化所有配置预处理器。
        /// </summary>
        private void InitializeProcessors()
        {
            _processors.Clear();
            var assembly = Assembly.GetExecutingAssembly();
            var processorTypes = assembly.GetTypes()
                .Where(t => typeof(IConfigProcessor).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in processorTypes)
            {
                RegisterProcessor(type);
            }
        }

        /// <summary>
        /// 实例化并注册单个配置预处理器。
        /// </summary>
        private void RegisterProcessor(System.Type type)
        {
            try
            {
                RegisterProcessorInternal(type);
            }
            catch (System.Exception ex)
            {
                Log.Error("Initialize config processor '{0}' failed: {1}", type.FullName, ex.ToString());
            }
        }

        /// <summary>
        /// 执行单个配置预处理器的实例化、逻辑执行与注册。
        /// </summary>
        private void RegisterProcessorInternal(System.Type type)
        {
            var processor = (IConfigProcessor)System.Activator.CreateInstance(type);
            processor.Process(_tables);
            _processors.Add(type, processor);
            Log.Info("Initialize config processor '{0}' success.", type.Name);
        }

        /// <summary>
        /// 资源加载失败回调。
        /// </summary>
        private void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            _byteBuf.Clear();
            throw new GameException($"load binary failure,file = {assetName}, status = {status}, error = {errorMessage}");
        }
    }
}