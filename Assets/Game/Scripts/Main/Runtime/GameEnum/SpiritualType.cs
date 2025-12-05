using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum SpiritualType
    {
        [InspectorName("金")] Gold = 1 << 0,
        [InspectorName("木")] Wood = 1 << 1,
        [InspectorName("水")] Water = 1 << 2,
        [InspectorName("火")] Fire = 1 << 3,
        [InspectorName("土")] Soil = 1 << 4,
        [InspectorName("雷")] Thunder = 1 << 5,
        [InspectorName("风")] Wind = 1 << 6,
        [InspectorName("山")] Mountain = 1 << 7,
        [InspectorName("天")] Sky = 1 << 8,
        [InspectorName("地")] Land = 1 << 9,
    }
}