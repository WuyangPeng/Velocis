// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Hotfix.HotfixCommon.Config;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Hotfix.HotfixBusiness.Procedure
{
    /// <summary>
    ///     预加载流程。
    /// </summary>
    public class ProcedurePreload : ProcedureBase
    {
        /// <summary>
        ///     存储各项资源是否加载完成的标记字典（键为资源名称，值为是否加载完成的布尔值）。
        /// </summary>
        private readonly Dictionary<string, bool> _loadedFlag = new();

        /// <summary>
        ///     获取是否使用原生对话框。
        /// </summary>
        public override bool UseNativeDialog => true;

        /// <summary>
        ///     流程进入时的初始化逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            _loadedFlag.Clear();

            PreloadResources();
        }

        /// <summary>
        ///     流程离开时的清理逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否正在关闭游戏框架。</param>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        ///     流程轮询更新逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，单位秒。</param>
        /// <param name="realElapseSeconds">真实流逝时间，单位秒。</param>
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_loadedFlag.Any(loadedFlag => !loadedFlag.Value))
            {
                return;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Start"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        /// <summary>
        ///     预加载所有资源（配置、数据表、字典、字体等）。
        /// </summary>
        private void PreloadResources()
        {
            LoadConfig("DefaultConfig");

            foreach (var dataTableName in PreloadDataTableNames.DataTableNames)
            {
                LoadDataTable(dataTableName);
            }

            LoadDictionary("Default");
            LoadFont("MainFont");
            LoadTMPFont("NotoSerifSC-Black");

            var gameConfig = new GameConfig();
            gameConfig.Initialize();
            GameEntry.GameConfig.SetConfigInstance(gameConfig);
        }

        /// <summary>
        ///     加载全局配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        private void LoadConfig(string configName)
        {
            var configAssetName = AssetUtility.GetConfigAsset(configName, false);
            _loadedFlag.Add(configAssetName, false);
            GameEntry.Config.ReadData(configAssetName, this);
        }

        /// <summary>
        ///     加载数据表。
        /// </summary>
        /// <param name="dataTableName">数据表名称。</param>
        private void LoadDataTable(string dataTableName)
        {
            var dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, false);
            _loadedFlag.Add(dataTableAssetName, false);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }

        /// <summary>
        ///     加载语言本地化字典。
        /// </summary>
        /// <param name="dictionaryName">字典名称。</param>
        private void LoadDictionary(string dictionaryName)
        {
            var dictionaryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
            _loadedFlag.Add(dictionaryAssetName, false);
            GameEntry.Localization.ReadData(dictionaryAssetName, this);
        }

        /// <summary>
        ///     加载系统原生字体。
        /// </summary>
        /// <param name="fontName">字体资源名称。</param>
        private void LoadFont(string fontName)
        {
            _loadedFlag.Add(Utility.Text.Format("Font.{0}", fontName), false);
            GameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
                (_, asset, _, _) =>
                {
                    _loadedFlag[Utility.Text.Format("Font.{0}", fontName)] = true;
                    UGuiForm.SetMainFont((Font)asset);
                    Log.Info("Load font '{0}' OK.", fontName);
                },
                (assetName, _, errorMessage, _) => { Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage); }));
        }

        /// <summary>
        ///     加载 TextMeshPro 字体资源。
        /// </summary>
        /// <param name="fontName">TextMeshPro 字体名称。</param>
        private void LoadTMPFont(string fontName)
        {
            _loadedFlag.Add(Utility.Text.Format("TMPFont.{0}", fontName), false);
            GameEntry.Resource.LoadAsset(AssetUtility.GetTMPFontAsset(fontName), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
                (_, asset, _, _) =>
                {
                    _loadedFlag[Utility.Text.Format("TMPFont.{0}", fontName)] = true;
                    UGuiForm.SetMainTMPFont((TMP_FontAsset)asset);
                    Log.Info("Load TMP font '{0}' OK.", fontName);
                },
                (assetName, _, errorMessage, _) =>
                {
                    Log.Warning("Can not load TMP font '{0}' from '{1}' with error message '{2}'. TextMeshPro will use default font.", fontName, assetName, errorMessage);
                    _loadedFlag[Utility.Text.Format("TMPFont.{0}", fontName)] = true;
                }));
        }

        /// <summary>
        ///     全局配置加载成功事件回调。
        /// </summary>
        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadConfigSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            _loadedFlag[ne.ConfigAssetName] = true;
            Log.Info("Load config '{0}' OK.", ne.ConfigAssetName);
        }

        /// <summary>
        ///     全局配置加载失败事件回调。
        /// </summary>
        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            var ne = (LoadConfigFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName, ne.ConfigAssetName, ne.ErrorMessage);
        }

        /// <summary>
        ///     数据表加载成功事件回调。
        /// </summary>
        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            _loadedFlag[ne.DataTableAssetName] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }

        /// <summary>
        ///     数据表加载失败事件回调。
        /// </summary>
        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            var ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        /// <summary>
        ///     语言字典加载成功事件回调。
        /// </summary>
        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            _loadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        /// <summary>
        ///     语言字典加载失败事件回调。
        /// </summary>
        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            var ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
    }
}