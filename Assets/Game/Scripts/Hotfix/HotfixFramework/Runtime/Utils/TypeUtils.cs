// 创建时间：2026-07-09
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Scripts.Hotfix.HotfixFramework.Runtime.Utils
{
    /// <summary>
    /// 类型工具类，用于在运行时从指定程序集（Assembly）中筛选并获取特定基类/接口的实现类型名称。
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// 获取实现了指定基类型（或接口）的所有具体类（非抽象类）的完整类型名称。
        /// 会同时检索运行时程序集（如 "Velocis.Runtime"）以及传入的程序集列表。
        /// </summary>
        /// <param name="typeBase">要查找的基类或接口类型。</param>
        /// <param name="assemblies">需要被检索的程序集列表。</param>
        /// <returns>排序后的符合条件的完整类型名称数组。</returns>
        public static string[] GetRuntimeTypeNames(Type typeBase, List<Assembly> assemblies)
        {
            var runtimeAssemblyNames = new[] { "Velocis.Runtime" };
            var typeNames = new List<string>();
            typeNames.AddRange(GetTypeNames(typeBase, runtimeAssemblyNames));
            typeNames.AddRange(GetTypeNames(typeBase, assemblies));
            typeNames.Sort();
            return typeNames.ToArray();
        }

        /// <summary>
        /// 从指定的程序集名称列表中加载程序集，并筛选其中实现了指定基类型（或接口）的所有具体类（非抽象类）的完整类型名称。
        /// </summary>
        /// <param name="typeBase">要查找的基类或接口类型。</param>
        /// <param name="assemblyNames">程序集名称数组。</param>
        /// <returns>符合条件的完整类型名称数组。</returns>
        private static string[] GetTypeNames(Type typeBase, string[] assemblyNames)
        {
            var typeNames = new List<string>();
            foreach (var assemblyName in assemblyNames)
            {
                var assembly = TryLoadAssembly(assemblyName);
                if (assembly == null)
                {
                    continue;
                }

                typeNames.AddRange(from type in assembly.GetTypes() where type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type) select type.FullName);
            }

            return typeNames.ToArray();
        }

        /// <summary>
        /// 尝试加载指定名称的程序集。
        /// </summary>
        /// <param name="assemblyName">程序集名称。</param>
        /// <returns>加载成功返回程序集实例，失败或异常返回 null。</returns>
        private static Assembly TryLoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                UnityGameFramework.Runtime.Log.Warning("Load assembly '{0}' failed with error: {1}", assemblyName, ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 从指定的已加载程序集列表中，筛选并获取实现了指定基类型（或接口）的所有具体类（非抽象类）的完整类型名称。
        /// </summary>
        /// <param name="typeBase">要查找的基类或接口类型。</param>
        /// <param name="assemblies">程序集列表。</param>
        /// <returns>符合条件的完整类型名称数组。</returns>
        private static string[] GetTypeNames(Type typeBase, List<Assembly> assemblies)
        {
            return (from assembly in assemblies from type in assembly.GetTypes() where type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type) select type.FullName).ToArray();
        }
    }
}