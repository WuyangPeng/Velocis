using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class TitleModule : ItemModule
    {
        public Dictionary<long, TitleData> Items { get; } = new();
        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

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
    }
}