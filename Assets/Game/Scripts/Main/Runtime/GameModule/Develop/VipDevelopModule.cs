using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Develop
{
    [Module]
    public class VipDevelopModule : DevelopModule
    {
        public Dictionary<long, DevelopData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }

        public void DeleteItem(long instanceId)
        {
            Items.Remove(instanceId);
        }
    }
}