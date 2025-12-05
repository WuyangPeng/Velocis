using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum InitGameType
    {
        [InspectorName("开始")] Begin = 0,

        [InspectorName("地图")] Map = 1,
        [InspectorName("家族")] Family = 2,
        [InspectorName("宗门")] Sect = 3,
        [InspectorName("Npc")] Npc = 4,
        [InspectorName("功法")] MartialArts = 5,

        [InspectorName("结束")] End = 6,
    }
}