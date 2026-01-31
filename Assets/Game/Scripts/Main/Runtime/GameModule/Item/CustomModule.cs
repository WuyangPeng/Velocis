using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class CustomModule : ItemModule
    {
        public Dictionary<long, CustomData> Items { get; } = new();
    }
}