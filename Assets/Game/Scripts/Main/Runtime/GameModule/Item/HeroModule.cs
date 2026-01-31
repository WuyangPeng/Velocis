using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class HeroModule : ItemModule
    {
        public Dictionary<long, HeroData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }

        public void DeleteItem(long itemId)
        {
            Items.Remove(itemId);
        }
    }
}