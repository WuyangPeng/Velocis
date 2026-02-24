using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class CustomModule : ItemModule
    {
        private Dictionary<long, CustomData> Items { get; } = new();
        private Dictionary<long, ItemSelectedData> SelectedItems { get; } = new();

        public void AddItem(CustomData item, bool isLogin)
        {
            Items[item.Inventory.ItemId] = item;

            if (!isLogin)
            {
                GameEntry.Event.Fire(this, ChangeCustomEventArgs.Create(item.Inventory.TemplateId));
            }
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

        public long GetItemCount(currency_type currency)
        {
            return Items.Where(element => element.Value.Inventory.TemplateId == (int)currency).Sum(element => element.Value.Inventory.Count);
        }
    }
}