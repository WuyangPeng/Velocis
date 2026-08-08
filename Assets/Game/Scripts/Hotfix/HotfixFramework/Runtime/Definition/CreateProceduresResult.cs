// 创建时间：2026-08-01
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using FrameworkProcedureBase = GameFramework.Procedure.ProcedureBase;
using ProcedureBase = Game.Scripts.Main.Runtime.Procedure.ProcedureBase;

namespace Game.Scripts.HotFix.HotfixFramework.Runtime.Definition
{
    /// <summary>
    ///     批量创建流程的返回结果结构体。
    /// </summary>
    public readonly struct CreateProceduresResult
    {
        /// <summary>
        ///     流程实例数组。
        /// </summary>
        public FrameworkProcedureBase[] Procedures { get; }

        /// <summary>
        ///     热更新入口流程实例。
        /// </summary>
        public ProcedureBase EntranceProcedure { get; }

        public CreateProceduresResult(FrameworkProcedureBase[] procedures, ProcedureBase entranceProcedure)
        {
            Procedures = procedures;
            EntranceProcedure = entranceProcedure;
        }

        /// <summary>
        ///     结果是否有效（流程数组及入口流程均不为空）。
        /// </summary>
        public bool IsValid => Procedures != null && EntranceProcedure != null;
    }
}