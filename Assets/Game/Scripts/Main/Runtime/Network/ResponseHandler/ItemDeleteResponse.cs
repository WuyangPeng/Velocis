using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ItemDeleteResponse : CeleritasHandlerBase<item_delete_response>
    {
        public override void Handle(object sender, header header, item_delete_response message)
        {
        }
    }
}