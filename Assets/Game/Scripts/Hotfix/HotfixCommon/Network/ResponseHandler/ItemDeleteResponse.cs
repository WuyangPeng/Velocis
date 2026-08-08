// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Config;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Item;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Game.Scripts.Main.Runtime.RuntimeException;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     道具删除响应处理器。
    /// </summary>
    public class ItemDeleteResponse : CeleritasHandlerBase<item_delete_response>
    {
        protected override void Handle(object sender, header header, item_delete_response message)
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in ItemDeleteResponse.");
            }

            var itemConfigContainer = GameEntry.GameConfig.GetTables().ItemConfigContainer;

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
                        case item_type.exp:
                        {
                            moduleComponent.GetModule<ExpModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.building:
                        {
                            moduleComponent.GetModule<BuildingModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.resource:
                        {
                            moduleComponent.GetModule<ResourceModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.soldier:
                        {
                            moduleComponent.GetModule<SoldierModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.machine:
                        {
                            moduleComponent.GetModule<MachineModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.skill_book:
                        {
                            moduleComponent.GetModule<SkillBookModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.blueprint:
                        {
                            moduleComponent.GetModule<BlueprintModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.gift_box:
                        {
                            moduleComponent.GetModule<GiftBoxModule>().DeleteItem(itemDeleteData.ItemId);
                        }
                            break;
                        case item_type.treasure:
                        {
                            moduleComponent.GetModule<TreasureModule>().DeleteItem(itemDeleteData.ItemId);
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