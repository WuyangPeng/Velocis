using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class AvatarModule : ItemModule
    {
        public Dictionary<long, AvatarData> Items { get; } = new();
        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        public AvatarData GetSelectedAvatar()
        {
            foreach (var itemSelectedData in SelectedItems.Where(itemSelectedData => itemSelectedData.Value.ItemType == item_type.avatar))
            {
                return Items.GetValueOrDefault(itemSelectedData.Value.SelectedId);
            }

            throw new GameException("avatar is empty.");
        }

        public void ClearItems()
        {
            Items.Clear();
            SelectedItems.Clear();
        }

        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }

        public void ClearSelectedItems()
        {
            SelectedItems.Clear();
        }

        public void AddOrUpdateSelectedItem(ItemSelectedData selectedData)
        {
            SelectedItems[selectedData.Id] = selectedData;
        }

        public void RemoveSelectedItem(long id)
        {
            SelectedItems.Remove(id);
        }

        public bool HasItem(int itemTemplateId)
        {
            return Items.Any(item => item.Value.Inventory.ItemId == itemTemplateId);
        }

        public AvatarData GetItem(int itemTemplateId)
        {
            return (from item in Items where item.Value.Inventory.ItemId == itemTemplateId select item.Value).FirstOrDefault();
        }
    }
}