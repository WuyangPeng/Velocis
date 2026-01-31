using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Item
{
    [Module]
    public class TitleModule : ItemModule
    {
        public Dictionary<long, TitleData> Items { get; } = new();
    }
}