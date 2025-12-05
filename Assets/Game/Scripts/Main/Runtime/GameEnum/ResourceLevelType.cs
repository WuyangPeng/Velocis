using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum ResourceLevelType  
    {
        [InspectorName("丰富")] Rich = 1,
        [InspectorName("一般")] Average = 2,
        [InspectorName("稀少")] Rare = 3,
        [InspectorName("贫乏")] Poverty = 4,
    }
}
 