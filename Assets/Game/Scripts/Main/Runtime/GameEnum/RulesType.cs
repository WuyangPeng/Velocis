using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum RulesType
    {
        [InspectorName("空")] Empty = 0x38,

        [InspectorName("守序")] Lawful = (0x01 << 0) + Empty,
        [InspectorName("逍遥")] Carefree = (0x01 << 1) + Empty,
        [InspectorName("混乱")] Chaos = (0x01 << 2) + Empty,
    }
}
 