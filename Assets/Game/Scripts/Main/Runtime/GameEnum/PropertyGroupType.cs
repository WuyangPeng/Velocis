using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum PropertyGroupType  
    {
        [InspectorName("基础")] Base = 100001,
        [InspectorName("默认")] Default = 100002,
        [InspectorName("战斗")] Battle = 100003,
        [InspectorName("内政")] Internal = 100004,
    }
}