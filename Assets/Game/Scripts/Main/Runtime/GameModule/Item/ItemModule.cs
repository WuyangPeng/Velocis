using Celeritas.Config;
using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    public class ItemModule : BaseModule
    {
        public void SendItemSelectedMessage(item_type itemType, item_selected_child_type childType, long operationId, int parameter, long selectedId)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientItem_ItemSelected();
            request.ItemSelected = new item_selected_data
            {
                ItemType = (int)itemType,
                ChildType = (int)childType,
                OperationId = (int)operationId,
                Parameter = parameter,
                SelectedId = selectedId
            };


            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendUnlockItemMessage(int itemId)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientItem_UnlockItem();
            request.TemplateId = itemId; 
            
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }
    }
}