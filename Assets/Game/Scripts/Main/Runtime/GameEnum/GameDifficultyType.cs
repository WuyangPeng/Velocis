using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{ 
    public enum GameDifficultyType
    {
        [InspectorName("凡人")] Mortal = 1,
        [InspectorName("修士")] Cultivator = 2,
        [InspectorName("真人")] TruePerson = 3,
        [InspectorName("仙尊")] ImmortalLord = 4,
        [InspectorName("天道")] HeavenlyDao = 5,
    }
}
 