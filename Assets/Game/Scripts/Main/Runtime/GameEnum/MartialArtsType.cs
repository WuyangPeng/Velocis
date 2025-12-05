using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum MartialArtsType  
    {
        [InspectorName("刀法")] Knife = 300001,
        [InspectorName("枪法")] Spear = 300002,
        [InspectorName("剑法")] Sword = 300003,
        [InspectorName("拳法")] Fist = 300004,
        [InspectorName("棍法")] Stick = 300005,
        [InspectorName("斧法")] Axe = 300006,
        [InspectorName("锤法")] Hammer = 300007,
        [InspectorName("鞭法")] Whip = 300008,
        [InspectorName("掌法")] Palm = 300009,
        [InspectorName("指法")] Finger = 300010,
    }
}