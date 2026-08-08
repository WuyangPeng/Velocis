// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     武将道具管理模块。
    /// </summary>
    [Module]
    public class HeroModule : ItemModule
    {
        /// <summary>
        ///     获取所有武将的数据字典。
        /// </summary>
        public Dictionary<long, HeroData> Items { get; } = new();

        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        ///     清理所有武将以及选择的数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        ///     删除指定的武将道具。
        /// </summary>
        /// <param name="itemId">武将道具ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        ///     清空所有武将道具的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        ///     添加或更新武将道具的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        ///     移除武将道具的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }
    }
}