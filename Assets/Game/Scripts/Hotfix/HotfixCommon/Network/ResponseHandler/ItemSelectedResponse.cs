// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Item;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     道具选中响应处理器。
    /// </summary>
    public class ItemSelectedResponse : CeleritasHandlerBase<item_selected_response>
    {
        private AvatarModule _avatarModule;
        private BlueprintModule _blueprintModule;
        private BuildingModule _buildingModule;
        private ConsumableModule _consumableModule;
        private CustomModule _customModule;
        private EquipmentModule _equipmentModule;
        private ExpModule _expModule;
        private FrameModule _frameModule;
        private GiftBoxModule _giftBoxModule;
        private HeroModule _heroModule;
        private MachineModule _machineModule;
        private ResourceModule _resourceModule;
        private SkillBookModule _skillBookModule;
        private SoldierModule _soldierModule;
        private TitleModule _titleModule;
        private TreasureModule _treasureModule;

        protected override void Handle(object sender, header header, item_selected_response message)
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

        /// <summary>
        /// 批量存储道具选中数据。
        /// </summary>
        /// <param name="selectedList">道具选中数据列表。</param>
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

        /// <summary>
        /// 确保并初始化道具选中状态相关的各个系统模块实例。
        /// </summary>
        /// <returns>若初始化成功返回 true，否则返回 false。</returns>
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
            _resourceModule ??= moduleComponent.GetModule<ResourceModule>();
            _soldierModule ??= moduleComponent.GetModule<SoldierModule>();
            _machineModule ??= moduleComponent.GetModule<MachineModule>();
            _skillBookModule ??= moduleComponent.GetModule<SkillBookModule>();
            _blueprintModule ??= moduleComponent.GetModule<BlueprintModule>();
            _giftBoxModule ??= moduleComponent.GetModule<GiftBoxModule>();
            _treasureModule ??= moduleComponent.GetModule<TreasureModule>();

            return true;
        }

        /// <summary>
        /// 登录刷新时，清理所有选中道具模块的数据。
        /// </summary>
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
            _resourceModule?.ClearSelectedItems();
            _soldierModule?.ClearSelectedItems();
            _machineModule?.ClearSelectedItems();
            _skillBookModule?.ClearSelectedItems();
            _blueprintModule?.ClearSelectedItems();
            _giftBoxModule?.ClearSelectedItems();
            _treasureModule?.ClearSelectedItems();

            Log.Info("ItemSelectedResponse: cleared all module selected item collections due to login refresh (IsLogin=true).");
        }

        /// <summary>
        /// 存储单个道具选中数据。
        /// </summary>
        /// <param name="selected">单个道具选中状态的详细数据。</param>
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
                case item_type.resource:
                    _resourceModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.soldier:
                    _soldierModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.machine:
                    _machineModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.skill_book:
                    _skillBookModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.blueprint:
                    _blueprintModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.gift_box:
                    _giftBoxModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.treasure:
                    _treasureModule.AddOrUpdateSelectedItem(itemSelectedData);
                    break;
                case item_type.none:
                default:
                    Log.Warning("ItemSelectedResponse: selected entry has unknown item type, skipping. Id={0}, ItemType={1}", selected.Id, selected.ItemType);
                    return;
            }
        }
    }
}