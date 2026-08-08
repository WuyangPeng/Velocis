// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     头像道具管理模块。
    /// </summary>
    [Module]
    public class AvatarModule : ItemModule
    {
        /// <summary>
        ///     获取所有头像的数据字典。
        /// </summary>
        public Dictionary<long, AvatarData> Items { get; } = new();

        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        /// <summary>
        ///     获取当前已选择的头像数据。
        /// </summary>
        /// <returns>头像数据实体。</returns>
        /// <exception cref="GameException">头像数据为空时抛出异常。</exception>
        public AvatarData GetSelectedAvatar()
        {
            foreach (var itemSelectedData in SelectedItems.Where(itemSelectedData => itemSelectedData.Value.ItemType == item_type.avatar))
            {
                return Items.GetValueOrDefault(itemSelectedData.Value.SelectedId);
            }

            throw new GameException("avatar is empty.");
        }

        /// <summary>
        ///     清理所有头像以及选中的头像数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        /// <summary>
        ///     删除指定的头像。
        /// </summary>
        /// <param name="itemId">头像道具唯一ID。</param>
        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        /// <summary>
        ///     清空所有头像的选择数据。
        /// </summary>
        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        /// <summary>
        ///     添加或更新头像的选择状态。
        /// </summary>
        /// <param name="selectedData">被选中的物品数据。</param>
        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        /// <summary>
        ///     移除头像的选择状态。
        /// </summary>
        /// <param name="id">选择记录的ID。</param>
        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }

        /// <summary>
        ///     检查当前是否拥有指定的头像模板 ID。
        /// </summary>
        /// <param name="itemTemplateId">头像配置ID。</param>
        /// <returns>若拥有则返回 true，否则返回 false。</returns>
        public bool HasItem(int itemTemplateId)
        {
            return Items.Any(item => item.Value.Inventory.ItemId == itemTemplateId);
        }

        /// <summary>
        ///     获取指定配置ID的头像数据。
        /// </summary>
        /// <param name="itemTemplateId">头像配置ID。</param>
        /// <returns>头像数据，若未拥有则返回 null。</returns>
        public AvatarData GetItem(int itemTemplateId)
        {
            return (from item in Items where item.Value.Inventory.ItemId == itemTemplateId select item.Value).FirstOrDefault();
        }
    }
}