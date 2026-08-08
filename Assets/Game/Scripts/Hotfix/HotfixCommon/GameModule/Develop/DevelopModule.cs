// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Client;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Develop
{
    /// <summary>
    ///     养成系统底层基类模块。
    /// </summary>
    public class DevelopModule : BaseModule
    {
        /// <summary>
        ///     发送升级请求
        /// </summary>
        /// <param name="systemId">系统ID</param>
        /// <param name="instanceId">实例ID</param>
        /// <param name="level">升级到的等级</param>
        public void SendDevelopLevelRequest(int systemId, long instanceId, int level)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientDevelop_DevelopLevel();
            request.Develop = new develop_data
            {
                SystemId = systemId,
                InstanceId = instanceId,
                Level = level
            };

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送重置请求
        /// </summary>
        /// <param name="systemId">系统ID</param>
        /// <param name="instanceId">实例ID</param>
        public void SendDevelopResetRequest(int systemId, long instanceId)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientDevelop_DevelopReset();
            request.Develop = new develop_data
            {
                SystemId = systemId,
                InstanceId = instanceId
            };

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }
    }
}