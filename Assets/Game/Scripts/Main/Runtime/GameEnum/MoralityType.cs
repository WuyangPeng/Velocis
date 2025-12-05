using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum MoralityType
    {
        [InspectorName("空")] Empty = 0x07,

        [InspectorName("仁德")] Benevolence = (0x01 << 3) + Empty,
        [InspectorName("中道")] Moderation = (0x01 << 4) + Empty,
        [InspectorName("诡诈")] Craftiness = (0x01 << 5) + Empty,
    }
}