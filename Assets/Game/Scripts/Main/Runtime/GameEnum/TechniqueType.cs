using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum TechniqueType
    {
        [InspectorName("炼丹")] Alchemy = 400001,
        [InspectorName("炼器")] ArtifactRefining = 400002,
        [InspectorName("符箓")] Talisman = 400003,
        [InspectorName("傀儡")] Puppet = 400004,
        [InspectorName("阵法")] Formation = 400005,
        [InspectorName("风水")] Geomancy = 400006,
    }
}