// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
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
    ///     道具数据更新（添加/修改）响应处理器。
    /// </summary>
    public class ItemResponse : CeleritasHandlerBase<item_response>
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

        private bool _isLogin;
        private MachineModule _machineModule;
        private ResourceModule _resourceModule;
        private SkillBookModule _skillBookModule;
        private SoldierModule _soldierModule;
        private TitleModule _titleModule;
        private TreasureModule _treasureModule;

        protected override void Handle(object sender, header header, item_response message)
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

        /// <summary>
        /// 批量存储道具数据到对应模块。
        /// </summary>
        /// <param name="inventoryList">道具数据列表。</param>
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

        /// <summary>
        /// 确保并初始化道具相关的各个系统模块实例。
        /// </summary>
        /// <returns>若初始化成功返回 true，否则返回 false。</returns>
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
        /// 登录刷新时，清理所有道具模块中已存在的数据。
        /// </summary>
        private void ClearAllItemModules()
        {
            _customModule?.ClearItems();
            _consumableModule?.ClearItems();
            _equipmentModule?.ClearItems();
            _avatarModule?.ClearItems();
            _frameModule?.ClearItems();
            _heroModule?.ClearItems();
            _titleModule?.ClearItems();
            _buildingModule?.ClearItems();
            _expModule?.ClearItems();
            _resourceModule?.ClearItems();
            _soldierModule?.ClearItems();
            _machineModule?.ClearItems();
            _skillBookModule?.ClearItems();
            _blueprintModule?.ClearItems();
            _giftBoxModule?.ClearItems();
            _treasureModule?.ClearItems();

            Log.Info("ItemResponse: cleared all module item collections due to login refresh (IsLogin=true).");
        }

        /// <summary>
        /// 存储单个道具数据，并将其分发到对应的模块中进行管理。
        /// </summary>
        /// <param name="inventory">单个道具的详细数据结构。</param>
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
                case inventory_data.PayloadOneofCase.Building:
                {
                    var data = new BuildingData(inventory.Clone().ToInventoryData());
                    if (inventory.Building != null)
                    {
                        data.Level = inventory.Building.Level;
                    }

                    _buildingModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Exp:
                {
                    var data = new ExpData(inventory.Clone().ToInventoryData());
                    _expModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Resource:
                {
                    var data = new ResourceData(inventory.Clone().ToInventoryData());
                    _resourceModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Soldier:
                {
                    var data = new SoldierData(inventory.Clone().ToInventoryData());
                    _soldierModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Machine:
                {
                    var data = new MachineData(inventory.Clone().ToInventoryData());
                    _machineModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.SkillBook:
                {
                    var data = new SkillBookData(inventory.Clone().ToInventoryData());
                    _skillBookModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Blueprint:
                {
                    var data = new BlueprintData(inventory.Clone().ToInventoryData());
                    _blueprintModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.GiftBox:
                {
                    var data = new GiftBoxData(inventory.Clone().ToInventoryData());
                    _giftBoxModule.Items[key] = data;
                    break;
                }
                case inventory_data.PayloadOneofCase.Treasure:
                {
                    var data = new TreasureData(inventory.Clone().ToInventoryData());
                    _treasureModule.Items[key] = data;
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