using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class FrameModule : ItemModule
    {
        public Dictionary<long, FrameData> Items { get; } = new();

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