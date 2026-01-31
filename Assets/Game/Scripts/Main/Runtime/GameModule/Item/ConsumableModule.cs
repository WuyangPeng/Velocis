using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class ConsumableModule : ItemModule
    {
        public Dictionary<long, ConsumableData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }
    }
}