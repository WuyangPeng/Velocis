using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.RuntimeException;
using GameFramework;
using System.Collections.Generic;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;

namespace Game.Scripts.Main.Runtime.GameData.User
{
    public class UserData
    {
        public int SaveIndex { get; set; } = 0;
        public GameDifficultyType GameDifficultyType { get; set; } = GameDifficultyType.Mortal;

        public int InitMapSize { get; set; }

        public int InitNpcCount { get; set; }

        public int InitSectCount { get; set; }

        public int InitFamilyCount { get; set; }

        public SexType SexType { get; set; } = SexType.Male;

        public int AvatarId { get; set; } = 0;

        public CampType CampType { get; set; } = CampType.CarefreeModeration;

        public RaceType RaceType { get; set; } = RaceType.Human;


        public int PropertyCount { get; set; } = Constant.Game.InitPropertyCount;

        public int SpiritualCount { get; set; } = Constant.Game.InitSpiritualCount;

        public int MartialArtsCount { get; set; } = Constant.Game.InitMartialArtsCount;

        public int TechniqueCount { get; set; } = Constant.Game.InitTechniqueCount;

        public HashSet<int> Talent { get; set; } = new();

        public string Name { get; set; } = GameEntry.Localization.GetString("Name.Default");

        public int Surname { get; set; } = 1;

        public bool InitWorld { get; set; } = false;

        public long FamilyId { get; set; }

        public long SectId { get; set; }

        private int age;

        public UserData()
        {

        }

        public void InitGameParameter()
        {
            var gameParameter = GameEntry.DataTable.GetDataTable<DRGameParameter>();
            var gameParameterRow = gameParameter.GetDataRow((int)GameParameterType.Middle);
            if (gameParameterRow == null) return;

            InitMapSize = Utility.Random.GetRandom(gameParameterRow.MinMapSize, gameParameterRow.MaxMapSize + 1);
            InitNpcCount = Utility.Random.GetRandom(gameParameterRow.MinNpcCount, gameParameterRow.MaxNpcCount + 1);
            InitSectCount = Utility.Random.GetRandom(gameParameterRow.MinSectCount, gameParameterRow.MaxSectCount + 1);
            InitFamilyCount = Utility.Random.GetRandom(gameParameterRow.MinFamilyCount, gameParameterRow.MaxFamilyCount + 1);

            PropertyCount = Constant.Game.InitPropertyCount;
            SpiritualCount = Constant.Game.InitSpiritualCount;
            MartialArtsCount = Constant.Game.InitMartialArtsCount;
            TechniqueCount = Constant.Game.InitTechniqueCount;
        }

        public void SetMapSize(GameParameterType gameParameterType)
        {
            var gameParameterRow = GetGameParameter(gameParameterType);

            InitMapSize = Utility.Random.GetRandom(gameParameterRow.MinMapSize, gameParameterRow.MaxMapSize + 1);
        }

        public void SetNpcCount(GameParameterType gameParameterType)
        {
            var gameParameterRow = GetGameParameter(gameParameterType);

            InitNpcCount = Utility.Random.GetRandom(gameParameterRow.MinNpcCount, gameParameterRow.MaxNpcCount + 1);
        }

        public void SetSectCount(GameParameterType gameParameterType)
        {
            var gameParameterRow = GetGameParameter(gameParameterType);

            InitSectCount = Utility.Random.GetRandom(gameParameterRow.MinSectCount, gameParameterRow.MaxSectCount + 1);
        }

        public void SetFamilyCount(GameParameterType gameParameterType)
        {
            var gameParameterRow = GetGameParameter(gameParameterType);

            InitFamilyCount = Utility.Random.GetRandom(gameParameterRow.MinFamilyCount, gameParameterRow.MaxFamilyCount + 1);
        }

        public DRGameParameter GetGameParameter(GameParameterType gameParameterType)
        {
            var gameParameter = GameEntry.DataTable.GetDataTable<DRGameParameter>();
            var row = gameParameter.GetDataRow((int)gameParameterType);
            return row ?? throw new GameException(Utility.Text.Format("Can not get game parameter '{0}' from data table.", gameParameterType.ToString()));
        }
        public void SetRulesType(RulesType rulesType)
        {
            var clean = (int)CampType & (int)RulesType.Empty;

            CampType = (CampType)(clean | ((int)rulesType & (int)MoralityType.Empty));
        }

        public void SetMoralityType(MoralityType moralityType)
        {
            var clean = (int)CampType & (int)MoralityType.Empty;

            CampType = (CampType)(clean | ((int)moralityType & (int)RulesType.Empty));
        }

        public RulesType GetRulesType()
        {
            return (RulesType)((int)RulesType.Empty | (int)CampType);
        }

        public MoralityType GetMoralityType()
        {
            return (MoralityType)((int)MoralityType.Empty | (int)CampType);
        }

        public void ReduceProperty()
        {
            --PropertyCount;
        }

        public void AddProperty()
        {
            ++PropertyCount;
        }

        public void ReduceSpiritual()
        {
            --SpiritualCount;
        }

        public void AddSpiritual()
        {
            ++SpiritualCount;
        }


        public void ReduceMartialArts()
        {
            --MartialArtsCount;
        }

        public void AddMartialArts()
        {
            ++MartialArtsCount;
        }

        public void ReduceTechnique()
        {
            --TechniqueCount;
        }

        public void AddTechnique()
        {
            ++TechniqueCount;
        }

        public bool HasSelectedTalent(int talentId)
        {
            return Talent.Contains(talentId);
        }

        public void AddTalent(int id)
        {
            Talent.Add(id);
        }

        public bool CanAddTalent(int id)
        {
            if (Talent.Count >= Constant.Game.MaxTalentCount)
            {
                return false;
            }

            return !Talent.Contains(id);
        }

        public bool HasSelectTalent()
        {
            return Talent.Count >= Constant.Game.MaxTalentCount;
        }

        public bool HasTalent(int id)
        {
            return Talent.Contains(id);
        }

        public void RemoveTalent(int id)
        {
            Talent.Remove(id);
        }
    }
}