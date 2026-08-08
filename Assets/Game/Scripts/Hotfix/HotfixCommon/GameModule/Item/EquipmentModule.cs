// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     装备道具管理模块。
    /// </summary>
    [Module]
    public class EquipmentModule : ItemModule
    {
        /// <summary>
        ///     获取所有装备的数据字典。
        /// </summary>
        public Dictionary<long, EquipmentData> Items { get; } = new();

        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        ///     清理所有装备以及选择的装备数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        ///     删除指定的装备。
        /// </summary>
        /// <param name="itemId">装备唯一ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        ///     清空所有装备的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        ///     添加或更新装备的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        ///     移除装备的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }
    }
}