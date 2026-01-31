using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Game.Scripts.Main.Runtime.RuntimeException;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ItemDeleteResponse : CeleritasHandlerBase<item_delete_response>
    {
        public override void Handle(object sender, header header, item_delete_response message)
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in ItemDeleteResponse.");
            }

            var itemConfigContainer = GameEntry.GameConfig.GetGameConfig().GetTables().ItemConfigContainer;

            foreach (var itemDeleteData in message.Data)
            {
                var iterConfig = itemConfigContainer.Get(itemDeleteData.TemplateId);
                if (iterConfig != null)
                {
                    switch (iterConfig.ItemType)
                    {
                        case item_type.custom:
                        {
                            moduleComponent.GetModule<CustomModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.consumable:
                        {
                            moduleComponent.GetModule<ConsumableModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.equipment:
                        {
                            moduleComponent.GetModule<EquipmentModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.avatar:
                        {
                            moduleComponent.GetModule<AvatarModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.frame:
                        {
                            moduleComponent.GetModule<FrameModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.title:
                        {
                            moduleComponent.GetModule<TitleModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.hero:
                        {
                            moduleComponent.GetModule<HeroModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.none:
                        default:
                            throw new GameException("Unknown item type.");
                    }
                }
            }
        }
    }
}