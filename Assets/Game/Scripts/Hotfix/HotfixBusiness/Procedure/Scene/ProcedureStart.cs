// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Hotfix.HotfixBusiness.Procedure.Scene
{
    /// <summary>
    ///     游戏开始/主菜单流程。
    /// </summary>
    public class ProcedureStart : ProcedureFormHostBase
    {
        /// <summary>
        ///     流程进入时的初始化逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            formComponent.AddForm(UIFormId.MainEntryForm);

            base.OnEnter(procedureOwner);

            GameEntry.ModuleComponent.ResetModule();

            Subscribe();
        }

        /// <summary>
        ///     订阅事件。
        /// </summary>
        private void Subscribe()
        {
            GameEntry.Event.Subscribe(CloseServerListEventArgs.EventId, OnServerListClose);
            GameEntry.Event.Subscribe(LoginLoadEventArgs.EventId, OnLoginLoad);
            GameEntry.Event.Subscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
        }

        /// <summary>
        ///     登录加载事件回调。
        /// </summary>
        private void OnLoginLoad(object sender, GameEventArgs e)
        {
            OpenUIForm(UIFormId.LoginLoadForm);
        }

        /// <summary>
        ///     服务器列表关闭事件回调。
        /// </summary>
        private static void OnServerListClose(object sender, GameEventArgs e)
        {
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.Clear();
        }

        /// <summary>
        ///     流程离开时的清理逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否正在关闭游戏框架。</param>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            Unsubscribe();

            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        ///     取消订阅事件。
        /// </summary>
        private void Unsubscribe()
        {
            GameEntry.Event.Unsubscribe(CloseServerListEventArgs.EventId, OnServerListClose);
            GameEntry.Event.Unsubscribe(LoginLoadEventArgs.EventId, OnLoginLoad);
            GameEntry.Event.Unsubscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
        }

        /// <summary>
        ///     网络连接断开事件回调。
        /// </summary>
        private static void OnNetworkClosed(object sender, GameEventArgs e)
        {
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.ClearCurrentLogin();
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

            if (nextSceneId <= 0)
            {
                return;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", nextSceneId); 
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        /// <summary>
        ///     开始游戏，准备切换到角色创建场景。
        /// </summary>
        public void StartGame()
        {
            nextSceneId = GameEntry.Config.GetInt("Scene.Create");
        }
    }
}