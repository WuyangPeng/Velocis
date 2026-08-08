// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     心跳包响应处理器，用于同步并更新客户端服务器时间。
    /// </summary>
    public class HeartbeatResponseHandler : CeleritasHandlerBase<heartbeat_response>
    {
        protected override void Handle(object sender, header header, heartbeat_response message)
        {
            Log.Info("ServerTime ='{0}'.", message.ServerTime);

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetServerTime(message.ServerTime);
        }
    }
}