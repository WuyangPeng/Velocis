// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    /// 自定义道具管理模块。
    /// </summary>
    [Module]
    public class CustomModule : ItemModule
    {
        private Dictionary<long, CustomData> Items { get; } = new();
        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        ///     添加或更新自定义道具，非登录时会派发数量变更事件。
        /// </summary>
        /// <param name="item">物品数据实体。</param>
        /// <param name="isLogin">是否在登录过程中加载数据。</param>
        public void AddItem(CustomData item, bool isLogin)
        {
            Items[item.Inventory.ItemId] = item;

            if (!isLogin)
            {
                GameEntry.Event.Fire(this, ChangeCustomEventArgs.Create(item.Inventory.TemplateId));
            }
        }

        /// <summary>
        ///     清理所有自定义道具以及选择的道具数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        ///     删除指定的自定义道具。
        /// </summary>
        /// <param name="itemId">道具唯一ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        ///     清空所有自定义道具的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        ///     添加或更新自定义道具的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        ///     移除自定义道具的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }

        /// <summary>
        ///     获取指定类型货币/代币的拥有数量。
        /// </summary>
        /// <param name="currency">货币类型。</param>
        /// <returns>当前拥有的代币数量。</returns>
        public long GetItemCount(currency_type currency)
        {
            return Items.Where(element => element.Value.Inventory.TemplateId == (int)currency).Sum(element => element.Value.Inventory.Count);
        }
    }
}