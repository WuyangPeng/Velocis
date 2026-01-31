using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class AvatarModule : ItemModule
    {
        public Dictionary<long, AvatarData> Items { get; } = new();

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