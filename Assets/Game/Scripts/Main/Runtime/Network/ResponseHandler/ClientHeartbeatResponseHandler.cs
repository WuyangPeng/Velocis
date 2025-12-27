using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ClientHeartbeatResponseHandler : CeleritasHandlerBase<client_heartbeat_response>
    {
        public override void Handle(object sender, header header, client_heartbeat_response message)
        {
            Log.Info("ServerTime ='{0}'.", message.ServerTime);

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetServerTime(message.ServerTime);
        }
    }
}