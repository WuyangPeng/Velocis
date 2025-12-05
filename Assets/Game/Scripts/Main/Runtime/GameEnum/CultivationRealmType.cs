using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{

    public enum CultivationRealmType
    {
        [InspectorName("炼气期")] QiRefining = 1,
        [InspectorName("筑基期")] FoundationBuilding = 2,
        [InspectorName("金丹期")] GoldenCore = 3,
        [InspectorName("元婴期")] NascentSoul = 4,
        [InspectorName("化神期")] SoulFormation = 5,
        [InspectorName("炼虚期")] VoidRefining = 6,
        [InspectorName("合体期")] BodyIntegration = 7,
        [InspectorName("大乘期")] Mahayana = 8,
        [InspectorName("渡劫期")] Transcendence = 9
    }

}