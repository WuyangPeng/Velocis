using System.Linq;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameData.User;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.User
{
    [Module]
    public class UserModule : BaseModule
    {
        private UserData userData = new();
        private PropertyData propertyData = new();

        public void SetGameDifficulty(GameDifficultyType gameDifficulty)
        {
            userData.GameDifficultyType = gameDifficulty;
        }

        public void Init()
        {
            userData.InitGameParameter();
            propertyData.Init();
        }

        public void SetMapSize(GameParameterType gameParameterType)
        {
            userData.SetMapSize(gameParameterType);
        }

        public void SetNpcCount(GameParameterType gameParameterType)
        {
            userData.SetNpcCount(gameParameterType);
        }

        public void SetSectCount(GameParameterType gameParameterType)
        {
            userData.SetSectCount(gameParameterType);
        }

        public void SetFamilyCount(GameParameterType gameParameterType)
        {
            userData.SetFamilyCount(gameParameterType);
        }

        public int GetInitMapSize()
        {
            return userData.InitMapSize;
        }

        public int GetInitNpcCount()
        {
            return userData.InitNpcCount;
        }

        public int GetInitSectCount()
        {
            return userData.InitSectCount;
        }

        public int GetInitFamilyCount()
        {
            return userData.InitFamilyCount;
        }

        public SexType GetSexType()
        {
            return userData.SexType;
        }

        public void SetSexType(SexType sexType)
        {
            userData.SexType = sexType;
        }

        public void SetAvatarId(int avatarId)
        {
            userData.AvatarId = avatarId;
        }

        public int GetAvatarId()
        {
            return userData.AvatarId;
        }

        public void SetRulesType(RulesType rulesType)
        {
            userData.SetRulesType(rulesType);
        }

        public void SetMoralityType(MoralityType moralityType)
        {
            userData.SetMoralityType(moralityType);
        }

        public RulesType GetRulesType()
        {
            return userData.GetRulesType();
        }

        public MoralityType GetMoralityType()
        {
            return userData.GetMoralityType();
        }

        public RaceType GetRaceType()
        {
            return userData.RaceType;
        }

        public void SetRaceType(RaceType raceType)
        {
            userData.RaceType = raceType;
        }

        public int GetPropertyCount()
        {
            return userData.PropertyCount;
        }
        public int GetSpiritualCount()
        {
            return userData.SpiritualCount;
        }
        public int GetInitBaseProperty(BasePropertyType basePropertyType)
        {
            var property = GameEntry.DataTable.GetDataTable<DRProperty>();

            var result = property.GetDataRow((int)basePropertyType).InitValue;

            var race = GameEntry.DataTable.GetDataTable<DRRace>();

            var raceRow = race.GetDataRow((int)GetRaceType());

            if (raceRow.PropertyId0 == (int)basePropertyType)
            {
                result += raceRow.PropertyChange0;
            }

            if (raceRow.PropertyId1 == (int)basePropertyType)
            {
                result += raceRow.PropertyChange1;
            }

            return result;
        }

        public int GetBaseProperty(BasePropertyType basePropertyType)
        {
            return propertyData.GetBaseProperty(basePropertyType) + GetInitBaseProperty(basePropertyType);
        }

        public int GetSpiritual(SpiritualType spiritual)
        {
            return propertyData.GetSpiritual(spiritual) + GetInitSpiritual(spiritual);
        }

        public static int GetInitSpiritual(SpiritualType spiritualId)
        {
            var spiritual = GameEntry.DataTable.GetDataTable<DRSpiritual>();

            return spiritual.GetDataRow((int)spiritualId).InitValue;
        }

        public int GetMartialArts(MartialArtsType martialArtsType)
        {
            return propertyData.GetMartialArts(martialArtsType) + GetInitMartialArts(martialArtsType);
        }

        public static int GetInitMartialArts(MartialArtsType martialArtsType)
        {
            var martialArts = GameEntry.DataTable.GetDataTable<DRMartialArts>();

            return martialArts.GetDataRow((int)martialArtsType).InitValue;
        }

        public int GetTechnique(TechniqueType techniqueType)
        {
            return propertyData.GetTechnique(techniqueType) + GetInitTechnique(techniqueType);
        }

        public static int GetInitTechnique(TechniqueType techniqueType)
        {
            var technique = GameEntry.DataTable.GetDataTable<DRTechnique>();

            return technique.GetDataRow((int)techniqueType).InitValue;
        }

        public void AddTechnique(int techniqueId)
        {
            userData.ReduceTechnique();
            propertyData.AddTechnique(techniqueId);
        }

        public void ReduceTechnique(int techniqueId)
        {
            userData.AddTechnique();
            propertyData.ReduceTechnique(techniqueId);
        }

        public void AddMartialArts(int martialArtsId)
        {
            userData.ReduceMartialArts();
            propertyData.AddMartialArts(martialArtsId);
        }

        public void ReduceMartialArts(int martialArtsId)
        {
            userData.AddMartialArts();
            propertyData.ReduceMartialArts(martialArtsId);
        }

        public void AddBaseProperty(int propertyId)
        {
            userData.ReduceProperty();
            propertyData.AddBaseProperty(propertyId);
        }

        public void ReduceBaseProperty(int propertyId)
        {
            userData.AddProperty();
            propertyData.ReduceBaseProperty(propertyId);

        }

        public void AddSpiritual(int spiritualId)
        {
            userData.ReduceSpiritual();
            propertyData.AddSpiritual(spiritualId);
        }

        public void ReduceSpiritual(int spiritualId)
        {
            userData.AddSpiritual();
            propertyData.ReduceSpiritual(spiritualId);

        }

        public bool HasSpiritual()
        {
            var spiritualTable = GameEntry.DataTable.GetDataTable<DRSpiritual>();

            return (from row in spiritualTable.GetAllDataRows() let spiritual = GetSpiritual((SpiritualType)row.Id) where row.EnableValue <= spiritual select row).Any();
        }

        public bool HasMartialArts()
        {
            var martialArtsTable = GameEntry.DataTable.GetDataTable<DRMartialArts>();

            return (from row in martialArtsTable.GetAllDataRows() let martialArts = GetMartialArts((MartialArtsType)row.Id) where row.Beginner <= martialArts select row).Any();
        }

        public int GetMartialArtsCount()
        {
            return userData.MartialArtsCount;
        }

        public int GetTechniqueCount()
        {
            return userData.TechniqueCount;
        }

        public bool HasTechnique()
        {
            var techniqueTable = GameEntry.DataTable.GetDataTable<DRTechnique>();

            return (from row in techniqueTable.GetAllDataRows() let technique = GetTechnique((TechniqueType)row.Id) where row.Beginner <= technique select row).Any();
        }

        public bool HasSelectedTalent(int talentId)
        {
            return userData.HasSelectedTalent(talentId);
        }

        public void AddTalent(int id)
        {
            userData.AddTalent(id);
        }

        public bool HasTalent(int id)
        {
            return userData.HasTalent(id);
        }

        public bool CanAddTalent(int id)
        {
            return userData.CanAddTalent(id);
        }

        public bool HasSelectTalent()
        {
            return userData.HasSelectTalent();
        }

        public int GetSaveIndex()
        {
            return userData.SaveIndex;
        }

        public void SetSaveIndex(int index)
        {
            userData.SaveIndex = index;
        }

        public GameDifficultyType GetGameDifficultyType()
        {
            return userData.GameDifficultyType;
        }

        public string GetFullName()
        {
            var surname = GameEntry.DataTable.GetDataTable<DRSurname>();

            return GameEntry.Localization.GetString(surname.GetDataRow(userData.Surname).Name) + userData.Name;
        }

        public string GetName()
        {
            return userData.Name;
        }

        public int GetSurname()
        {
            return userData.Surname;
        }

        public void RemoveTalent(int id)
        {
            userData.RemoveTalent(id);
        }

        public void SetName(string playerName)
        {
            userData.Name = playerName;
        }

        public void SetSurname(int surname)
        {
            userData.Surname = surname;
        }

        public bool IsInitWorld()
        {
            return userData.InitWorld;
        }

        public void SetInitWorld()
        {
            userData.InitWorld = true;
        }

        public PropertyData GetPropertyData()
        {
            return propertyData;
        }

        public UserData GetUserData()
        {
            return userData;
        }

        public void Init(UserData initUserData, PropertyData initPropertyData)
        {
            userData = initUserData;
            propertyData = initPropertyData;
        }

        public void SetFamilyId(long id)
        {
            userData.FamilyId = id;
        }

        public void SetSect(long id)
        {
            userData.SectId = id;
        }

        public long GetFamilyId()
        {
            return userData.FamilyId;
        }

        public long GetSectId()
        {
            return userData.SectId;
        }
    }
}
