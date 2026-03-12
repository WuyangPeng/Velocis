using System;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class ItemSelectedResponse : CeleritasHandlerBase<item_selected_response>
    {
        private AvatarModule _avatarModule;
        private BuildingModule _buildingModule;
        private ConsumableModule _consumableModule;
        private CustomModule _customModule;
        private EquipmentModule _equipmentModule;
        private ExpModule _expModule;
        private FrameModule _frameModule;
        private HeroModule _heroModule;
        private TitleModule _titleModule;

        public override void Handle(object sender, header header, item_selected_response message)
        {
            if (message.ItemSelected.Count == 0)
            {
                return;
            }

            if (!EnsureModules())
            {
                return;
            }

            if (message.IsLogin)
            {
                ClearAllSelectedItemModules();
            }

            StoreSelectedItem(message.ItemSelected);
        }

        private void StoreSelectedItem(RepeatedField<item_selected_data> selectedList)
        {
            foreach (var selected in selectedList)
            {
                try
                {
                    StoreSelectedItem(selected);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to process selected item: {0}", ex.Message);
                }
            }
        }

        private bool EnsureModules()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in ItemSelectedResponse.EnsureModules.");
                return false;
            }

            _customModule ??= moduleComponent.GetModule<CustomModule>();
            _consumableModule ??= moduleComponent.GetModule<ConsumableModule>();
            _equipmentModule ??= moduleComponent.GetModule<EquipmentModule>();
            _avatarModule ??= moduleComponent.GetModule<AvatarModule>();
            _frameModule ??= moduleComponent.GetModule<FrameModule>();
            _heroModule ??= moduleComponent.GetModule<HeroModule>();
            _titleModule ??= moduleComponent.GetModule<TitleModule>();
            _buildingModule ??= moduleComponent.GetModule<BuildingModule>();
            _expModule ??= moduleComponent.GetModule<ExpModule>();

            return true;
        }

        private void ClearAllSelectedItemModules()
        {
            _customModule?.ClearSelectedItems();
            _consumableModule?.ClearSelectedItems();
            _equipmentModule?.ClearSelectedItems();
            _avatarModule?.ClearSelectedItems();
            _frameModule?.ClearSelectedItems();
            _heroModule?.ClearSelectedItems();
            _titleModule?.ClearSelectedItems();
            _buildingModule?.ClearSelectedItems();
            _expModule?.ClearSelectedItems();

            Log.Info("ItemSelectedResponse: cleared all module selected item collections due to login refresh (IsLogin=true).");
        }

        private void StoreSelectedItem(item_selected_data selected)
        {
            if (selected.Id == 0)
            {
                Log.Warning("ItemSelectedResponse: ignoring selected entry with no Id: {0}", selected.ToString());
                return;
            }

            var itemType = (item_type)selected.ItemType;

            var itemSelectedData = new ItemSelectedData(
                selected.Id,
                itemType,
                (item_selected_child_type)selected.ChildType,
                selected.OperationId,
                selected.Parameter,
                selected.SelectedId
            );

            switch (itemType)
            {
                case item_type.custom:
                    _customModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.consumable:
                    _consumableModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.equipment:
                    _equipmentModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.avatar:
                    _avatarModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.frame:
                    _frameModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.title:
                    _titleModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.hero:
                    _heroModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.building:
                    _buildingModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.exp:
                    _expModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.none:
                default:
                    Log.Warning("ItemSelectedResponse: selected entry has unknown item type, skipping. Id={0}, ItemType={1}", selected.Id, selected.ItemType);
                    return;
            }
        }
    }
}