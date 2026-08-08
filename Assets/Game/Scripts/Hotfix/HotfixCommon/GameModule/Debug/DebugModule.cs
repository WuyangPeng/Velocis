// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Client;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Debug
{
    /// <summary>
    ///     调试模块，用于发送调试相关的协议与控制调试状态。
    /// </summary>
    [Module]
    public class DebugModule : BaseModule
    {
        /// <summary>
        ///     获取或设置是否开启调试状态。
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        ///     发送调试指令消息给服务器。
        /// </summary>
        /// <param name="type">调试指令类型。</param>
        /// <param name="id">调试对象编号/目标ID。</param>
        /// <param name="parameter">参数。</param>
        public void SendDebugMessage(debug_type type, long id, long parameter)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientDebug_Debug();
            request.Type = type;
            request.Id = id;
            request.Parameter = parameter;

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }
    }
}