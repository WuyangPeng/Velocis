// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     建筑道具管理模块。
    /// </summary>
    [Module]
    public class BuildingModule : ItemModule
    {
        /// <summary>
        ///     获取所有建筑的数据字典。
        /// </summary>
        public Dictionary<long, BuildingData> Items { get; } = new();

        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        ///     清理所有建筑以及选择的建筑数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        ///     删除指定的建筑。
        /// </summary>
        /// <param name="itemId">建筑道具唯一ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        ///     清空所有建筑的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        ///     添加或更新建筑的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        ///     移除建筑的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }
    }
}