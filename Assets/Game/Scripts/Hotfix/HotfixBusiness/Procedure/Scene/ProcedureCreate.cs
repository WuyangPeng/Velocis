// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using Game.Scripts.Hotfix.HotfixCommon.Definition;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Hotfix.HotfixBusiness.Procedure.Scene
{
    /// <summary>
    ///     角色创建流程。
    /// </summary>
    public class ProcedureCreate : ProcedureFormHostBase
    {
        /// <summary>
        ///     进入游戏，开始切换到初始游戏场景。
        /// </summary>
        public void EnterGame()
        {
        }

        /// <summary>
        ///     返回主菜单，断开当前网络连接，并准备切换到主菜单场景。
        /// </summary>
        public void ReturnMenu()
        {
            nextSceneId = GameEntry.Config.GetInt("Scene.Start");
            CloseNetworkChannel(NetworkConstant.TcpChannel);
        }

        /// <summary>
        ///     保存游戏数据。当前暂无具体实现。
        /// </summary>
        public void SaveData()
        {
        }

        /// <summary>
        ///     流程进入时的初始化逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            // 打开选择名字场景

            base.OnEnter(procedureOwner);
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

            if (nextSceneId == 0)
            {
                return;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", nextSceneId);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        /// <summary>
        ///     关闭指定的网络通道。
        /// </summary>
        /// <param name="channelName">网络通道名称。</param>
        private static void CloseNetworkChannel(string channelName)
        {
            var channel = GameEntry.Network.GetNetworkChannel(channelName);
            if (channel is { Connected: true })
            {
                channel.Close();
            }
        }
    }
}