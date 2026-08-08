// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Hotfix.HotfixBusiness.Procedure
{
    /// <summary>
    /// 包含 UI 窗体管理的流程基类。
    /// </summary>
    public abstract class ProcedureFormHostBase : ProcedureBase, IProcedureFormHost
    {
        /// <summary>
        /// 流程持有的 UI 窗体管理组件。
        /// </summary>
        protected readonly FormComponent formComponent = new();

        /// <summary>
        /// 下一个要切换的目标场景 ID。为 0 时表示不切换。
        /// </summary>
        protected int nextSceneId;

        /// <summary>
        /// 获取是否使用原生对话框。
        /// </summary>
        public override bool UseNativeDialog => false;

        /// <summary>
        /// 打开指定的 UI 窗体。
        /// </summary>
        /// <param name="form">要打开的 UI 窗体 ID。</param>
        public void OpenUIForm(UIFormId form)
        {
            formComponent.OpenUIForm(form);
        }

        /// <summary>
        /// 关闭并移除指定的 UI 窗体。
        /// </summary>
        /// <param name="formId">要移除的 UI 窗体 ID。</param>
        public void RemoveUIForm(UIFormId formId)
        {
            formComponent.RemoveUIForm(formId);
        }

        /// <summary>
        /// 流程进入时的初始化逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            nextSceneId = 0;
            formComponent.OnEnter(procedureOwner);
        }

        /// <summary>
        /// 流程离开时的清理逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否正在关闭游戏框架。</param>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            formComponent.OnLeave(procedureOwner, isShutdown);
        }
    }
}
