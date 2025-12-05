using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.GameData.User;
using Game.Scripts.Main.Runtime.GameEnum;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class NpcBaseData
    {
        public long ID { get; set; }

        public SexType SexType { get; set; } = SexType.Male;

        public int AvatarId { get; set; } = 0;

        public CampType CampType { get; set; } = CampType.CarefreeModeration;

        public RaceType RaceType { get; set; } = RaceType.Human;


        public int PropertyCount { get; set; } = Constant.Game.InitPropertyCount;

        public int SpiritualCount { get; set; } = Constant.Game.InitSpiritualCount;

        public int MartialArtsCount { get; set; } = Constant.Game.InitMartialArtsCount;

        public int TechniqueCount { get; set; } = Constant.Game.InitTechniqueCount;

        public HashSet<int> Talent { get; set; } = new();

        public int Surname { get; set; } = 1;

        public int Name { get; set; } = 1;

        private readonly PropertyData propertyData = new PropertyData();


        public long FamilyId { get; set; }

        public long SectId { get; set; }
    }
}