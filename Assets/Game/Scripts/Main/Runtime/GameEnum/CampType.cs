using UnityEngine;

namespace Game.Scripts.Main.Runtime.GameEnum
{
    public enum CampType
    {
        [InspectorName("守序仁德")] LawfulBenevolence = RulesType.Lawful & MoralityType.Benevolence,
        [InspectorName("守序中道")] LawfulModeration = RulesType.Lawful & MoralityType.Moderation,
        [InspectorName("守序诡诈")] LawfulCraftiness = RulesType.Lawful & MoralityType.Craftiness,

        [InspectorName("逍遥仁德")] CarefreeBenevolence = RulesType.Carefree & MoralityType.Benevolence,
        [InspectorName("逍遥中道")] CarefreeModeration = RulesType.Carefree & MoralityType.Moderation,
        [InspectorName("逍遥诡诈")] CarefreeCraftiness = RulesType.Carefree & MoralityType.Craftiness,

        [InspectorName("混乱仁德")] ChaosBenevolence = RulesType.Chaos & MoralityType.Benevolence,
        [InspectorName("混乱中道")] ChaosModeration = RulesType.Chaos & MoralityType.Moderation,
        [InspectorName("混乱诡诈")] ChaosCraftiness = RulesType.Chaos & MoralityType.Craftiness, 
    }
}