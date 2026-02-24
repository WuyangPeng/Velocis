using System;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ItemResponse : CeleritasHandlerBase<item_response>
    {
        private AvatarModule _avatarModule;
        private ConsumableModule _consumableModule;
        private CustomModule _customModule;
        private EquipmentModule _equipmentModule;
        private FrameModule _frameModule;
        private HeroModule _heroModule;

        private bool _isLogin;
        private TitleModule _titleModule;

        public override void Handle(object sender, header header, item_response message)
        {
            _isLogin = message.IsLogin;

            if (message.Inventory.Count == 0)
            {
                return;
            }

            if (!EnsureModules())
            {
                return;
            }

            if (_isLogin)
            {
                ClearAllItemModules();
            }

            StoreInventoryItem(message.Inventory);
        }

        private void StoreInventoryItem(RepeatedField<inventory_data> inventoryList)
        {
            foreach (var inventory in inventoryList)
            {
                try
                {
                    StoreInventoryItem(inventory);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to process inventory item: {0}", ex.Message);
                }
            }
        }

        private bool EnsureModules()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in ItemResponse.EnsureModules.");
                return false;
            }

            _customModule ??= moduleComponent.GetModule<CustomModule>();
            _consumableModule ??= moduleComponent.GetModule<ConsumableModule>();
            _equipmentModule ??= moduleComponent.GetModule<EquipmentModule>();
            _avatarModule ??= moduleComponent.GetModule<AvatarModule>();
            _frameModule ??= moduleComponent.GetModule<FrameModule>();
            _heroModule ??= moduleComponent.GetModule<HeroModule>();
            _titleModule ??= moduleComponent.GetModule<TitleModule>();

            return true;
        }

        private void ClearAllItemModules()
        {
            _customModule?.ClearItems();
            _consumableModule?.ClearItems();
            _equipmentModule?.ClearItems();
            _avatarModule?.ClearItems();
            _frameModule?.ClearItems();
            _heroModule?.ClearItems();
            _titleModule?.ClearItems();

            Log.Info("ItemResponse: cleared all module item collections due to login refresh (IsLogin=true).");
        }

        private void StoreInventoryItem(inventory_data inventory)
        {
            if (inventory.ItemId == 0)
            {
                Log.Warning("ItemResponse: ignoring inventory entry with no ItemId: {0}", inventory.ToString());
                return;
            }

            var key = inventory.ItemId;

            switch (inventory.PayloadCase)
            {
                case inventory_data.PayloadOneofCase.Custom:
                {
                    var data = new CustomData(inventory.Clone().ToInventoryData());
                    _customModule.AddItem(data, _isLogin);
                    break;
                }
                case inventory_data.PayloadOneofCase.Consumable:
                {
                    var data = new ConsumableData(inventory.Clone().ToInventoryData());
                    if (inventory.Consumable != null)
                    {
                        data.ExpireTime = inventory.Consumable.ExpireTime;
                    }

                    _consumableModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Equipment:
                {
                    var data = new EquipmentData(inventory.Clone().ToInventoryData());
                    if (inventory.Equipment != null)
                    {
                        data.Strength = inventory.Equipment.Strength;
                        data.Durability = inventory.Equipment.Durability;
                    }

                    _equipmentModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Avatar:
                {
                    var data = new AvatarData(inventory.Clone().ToInventoryData());
                    _avatarModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Frame:
                {
                    var data = new FrameData(inventory.Clone().ToInventoryData());
                    _frameModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Title:
                {
                    var data = new TitleData(inventory.Clone().ToInventoryData());
                    _titleModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Hero:
                {
                    var data = new HeroData(inventory.Clone().ToInventoryData());
                    _heroModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.None:
                default:
                {
                    Log.Warning("ItemResponse: inventory entry has no payload, skipping. ItemId={0}, TemplateId={1}", inventory.ItemId, inventory.TemplateId);
                    return;
                }
            }
        }
    }
}