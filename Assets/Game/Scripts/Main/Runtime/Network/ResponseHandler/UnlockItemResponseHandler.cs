using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class UnlockItemResponseHandler : CeleritasHandlerBase<unlock_item_response>
    {
        public override void Handle(object sender, header header, unlock_item_response message)
        {
        }
    }
}