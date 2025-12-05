using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{

    public enum BasePropertyType
    {
        [InspectorName("力道")] Strength = 200001,
        [InspectorName("身法")] Agile = 200002,
        [InspectorName("根骨")] Constitution = 200003,
        [InspectorName("悟性")] Intelligence = 200004,
        [InspectorName("神识")] Perception = 200005,
        [InspectorName("灵韵")] Charm = 200006,
    }
}