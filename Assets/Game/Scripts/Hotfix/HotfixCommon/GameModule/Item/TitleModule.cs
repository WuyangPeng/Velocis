// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    /// 称号道具管理模块。
    /// </summary>
    [Module]
    public class TitleModule : ItemModule
    {
        /// <summary>
        /// 获取所有称号的数据字典。
        /// </summary>
        public Dictionary<long, TitleData> Items { get; } = new();

        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        /// 清理所有称号以及选择的数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        /// 删除指定的称号。
        /// </summary>
        /// <param name="itemId">称号道具唯一ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        /// 清空所有称号的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        /// 添加或更新称号的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        /// 移除称号的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }

        /// <summary>
        /// 获取当前选中的称号数据。
        /// </summary>
        /// <returns>选中的称号数据，若未选中任何称号则返回 null。</returns>
        public TitleData GetSelectedTitle()
        {
            return SelectedItems.Where(itemSelectedData => itemSelectedData.Value.ItemType == item_type.title).Select(itemSelectedData => Items.GetValueOrDefault(itemSelectedData.Value.SelectedId)).FirstOrDefault();
        }

        /// <summary>
        /// 检查当前是否拥有指定的称号配置 ID。
        /// </summary>
        /// <param name="itemTemplateId">配置ID。</param>
        /// <returns>若拥有则返回 true，否则返回 false。</returns>
        public bool HasItem(int itemTemplateId)
        {
            return Items.Any(item => item.Value.Inventory.ItemId == itemTemplateId);
        }

        /// <summary>
        /// 获取指定配置ID的称号数据。
        /// </summary>
        /// <param name="itemTemplateId">配置ID。</param>
        /// <returns>称号数据，若未拥有则返回 null。</returns>
        public TitleData GetItem(int itemTemplateId)
        {
            return (from item in Items where item.Value.Inventory.ItemId == itemTemplateId select item.Value).FirstOrDefault();
        }
    }
}