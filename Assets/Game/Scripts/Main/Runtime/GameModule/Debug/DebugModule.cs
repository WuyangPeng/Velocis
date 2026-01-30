using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Main.Runtime.GameModule.Debug
{
    [Module]
    public class DebugModule : BaseModule
    {
        public bool IsDebug { get; set; } = false;

        public void SendDebugMessage(debug_type type, long id, long parameter)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientDebug_Debug();
            request.Type = type;
            request.Id = id;
            request.Parameter = parameter;

            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }
    }
}