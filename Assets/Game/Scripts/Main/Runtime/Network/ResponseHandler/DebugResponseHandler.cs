using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class DebugResponseHandler : CeleritasHandlerBase<debug_response>
    {
        public override void Handle(object sender, header header, debug_response message)
        {
        }
    }
}