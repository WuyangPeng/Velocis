// 创建时间：2026-08-01
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System;
using System.Collections.Generic;
using System.Reflection;
using Game.Scripts.Hotfix.HotfixCommon.Platform;
using Game.Scripts.HotFix.HotfixFramework.Runtime.Definition;
using Game.Scripts.Hotfix.HotfixFramework.Runtime.Utils;
using Game.Scripts.Main.Runtime.Platform;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using FrameworkProcedureBase = GameFramework.Procedure.ProcedureBase;
using ProcedureBase = Game.Scripts.Main.Runtime.Procedure.ProcedureBase;

namespace Game.Scripts.HotFix.HotfixFramework.Runtime
{
    /// <summary>
    ///     热更新代码的入口类。
    ///     负责接收主工程加载完毕的热更新程序集，并初始化/重置游戏流程（Procedure），使游戏从主工程过渡到热更新业务逻辑中。
    /// </summary>
    public static class HotfixEntry
    {
        private const string EntranceProcedureTypeName = "Game.Scripts.Hotfix.HotfixBusiness.Procedure.ProcedurePreload";

        private static List<Assembly> _hotfixAssemblies;

        /// <summary>
        ///     热更新入口方法，由主工程加载完 DLL 后通过反射或委托调用。
        /// </summary>
        /// <param name="objects">参数列表。objects[0] 期望为 List{Assembly}，即加载好的热更新程序集列表。</param>
        public static void Entrance(object[] objects)
        {
            _hotfixAssemblies = (List<Assembly>)objects[0];
            PlatformUtility.ApplyDisplaySettings();
            ResetProcedure();
        }

        /// <summary>
        ///     重置游戏流程。
        ///     销毁主工程初始化时的流程状态机，重新扫描热更新程序集中的所有流程类，并以 Preload 流程作为新入口重新启动流程状态机。
        /// </summary>
        private static void ResetProcedure()
        {
            if (!EditorPlatformUtility.HasHotfixAssemblies(_hotfixAssemblies))
            {
                return;
            }

            var fsmManager = GameFrameworkEntry.GetModule<IFsmManager>();
            DestroyProcedureFsm(fsmManager);
            RecreateAndStartProcedures(fsmManager);
        }

        /// <summary>
        ///     重新创建并启动新的热更新流程。
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器。</param>
        private static void RecreateAndStartProcedures(IFsmManager fsmManager)
        {
            var procedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
            var procedureTypeNames = TypeUtils.GetRuntimeTypeNames(typeof(ProcedureBase), _hotfixAssemblies);
            var result = CreateProcedures(procedureTypeNames);
            if (!result.IsValid)
            {
                return;
            }

            procedureManager.Initialize(fsmManager, result.Procedures);
            procedureManager.StartProcedure(result.EntranceProcedure.GetType());
        }

        /// <summary>
        ///     销毁现有的流程有限状态机（FSM）。
        ///     重置流程前，必须先销毁由主工程创建的旧状态机，以释放旧流程所占用的资源。
        /// </summary>
        /// <param name="fsmManager">状态机管理器。</param>
        private static void DestroyProcedureFsm(IFsmManager fsmManager)
        {
            if (fsmManager != null)
            {
                GameEntry.Fsm.DestroyFsm<IProcedureManager>();
            }
        }

        /// <summary>
        ///     批量尝试实例化所有获取到的流程，并获取热更新游戏入口流程结果。
        /// </summary>
        /// <param name="procedureTypeNames">待创建的流程类型名称数组。</param>
        /// <returns>流程实例化与筛选结果。</returns>
        private static CreateProceduresResult CreateProcedures(string[] procedureTypeNames)
        {
            var procedures = new FrameworkProcedureBase[procedureTypeNames.Length];
            var entranceProcedure = TryCreateAndRecordProcedures(procedureTypeNames, procedures);
            return entranceProcedure != null ? new CreateProceduresResult(procedures, entranceProcedure) : default;
        }

        /// <summary>
        ///     循环实例化所有流程，填充数组，并返回入口流程实例。
        /// </summary>
        /// <param name="procedureTypeNames">待创建的流程类型名称数组。</param>
        /// <param name="procedures">目标流程实例数组（在此被填充）。</param>
        /// <returns>返回创建成功的入口流程实例；如果任何一个流程创建失败或未找到入口流程，则返回 null。</returns>
        private static ProcedureBase TryCreateAndRecordProcedures(string[] procedureTypeNames, FrameworkProcedureBase[] procedures)
        {
            ProcedureBase entranceProcedure = null;

            for (var i = 0; i < procedureTypeNames.Length; i++)
            {
                var typeName = procedureTypeNames[i];
                var procedure = TryCreateAndRecordProcedure(typeName, i, procedures);
                if (procedure == null)
                {
                    return null;
                }

                if (EntranceProcedureTypeName == typeName)
                {
                    entranceProcedure = procedure;
                }
            }

            if (entranceProcedure == null)
            {
                Log.Error("Entrance procedure is invalid.");
            }

            return entranceProcedure;
        }

        /// <summary>
        ///     创建单个流程并记录到数组中。
        /// </summary>
        /// <param name="typeName">待创建的流程类型名称。</param>
        /// <param name="index">流程在数组中的索引。</param>
        /// <param name="procedures">目标流程实例数组。</param>
        /// <returns>若实例化并记录成功则返回流程对象，否则返回 null。</returns>
        private static ProcedureBase TryCreateAndRecordProcedure(string typeName,
            int index,
            FrameworkProcedureBase[] procedures)
        {
            var procedure = TryCreateProcedure(typeName);
            if (procedure == null)
            {
                return null;
            }

            procedures[index] = procedure;
            return procedure;
        }

        /// <summary>
        ///     尝试根据类型名称实例化单个流程。
        /// </summary>
        /// <param name="typeName">流程的完整类型名称。</param>
        /// <returns>实例化后的流程对象；如果失败，则返回 null。</returns>
        private static ProcedureBase TryCreateProcedure(string typeName)
        {
            var procedureType = Utility.Assembly.GetType(typeName);
            if (procedureType == null)
            {
                Log.Error("Can not find procedure type '{0}'.", typeName);
                return null;
            }

            var procedure = (ProcedureBase)Activator.CreateInstance(procedureType);
            if (procedure != null)
            {
                return procedure;
            }

            Log.Error("Can not create procedure instance '{0}'.", typeName);
            return null;
        }
    }
}