using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ClientHeartbeatResponseHandler : CeleritasHandlerBase<client_heartbeat_response>
    {
        public override void Handle(object sender, client_heartbeat_response message)
        {
            Log.Info("ServerTime ='{0}'.", message.ServerTime);
        }
    }
}