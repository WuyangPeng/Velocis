using System;
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
        private long _clientTime;
        private PropertyData _propertyData = new();
        private long _serverTime;
        private long _serverTimeOffset;
        private UserData _userData = new();
        private long _userId;

        public void SetServerTime(long serverTime)
        {
            _serverTime = serverTime;
            _clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _serverTimeOffset = serverTime - _clientTime;
        }

        public void SetUserId(long userId)
        {
            _userId = userId;
        }

        public long GetUserId()
        {
            return _userId;
        }

        public long GetCurrentServerTime()
        {
            var currentLocalTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var currentServerTimestamp = currentLocalTimestamp + _serverTimeOffset;

            return currentServerTimestamp;
        }

        public void Init()
        {
            _userData.InitGameParameter();
            _propertyData.Init();
        }

        public void SetGameDifficulty(GameDifficultyType gameDifficulty)
        {
            _userData.GameDifficultyType = gameDifficulty;
        }

        public void SetMapSize(GameParameterType gameParameterType)
        {
            _userData.SetMapSize(gameParameterType);
        }

        public void SetNpcCount(GameParameterType gameParameterType)
        {
            _userData.SetNpcCount(gameParameterType);
        }

        public void SetSectCount(GameParameterType gameParameterType)
        {
            _userData.SetSectCount(gameParameterType);
        }

        public void SetFamilyCount(GameParameterType gameParameterType)
        {
            _userData.SetFamilyCount(gameParameterType);
        }

        public int GetInitMapSize()
        {
            return _userData.InitMapSize;
        }

        public int GetInitNpcCount()
        {
            return _userData.InitNpcCount;
        }

        public int GetInitSectCount()
        {
            return _userData.InitSectCount;
        }

        public int GetInitFamilyCount()
        {
            return _userData.InitFamilyCount;
        }

        public SexType GetSexType()
        {
            return _userData.SexType;
        }

        public void SetSexType(SexType sexType)
        {
            _userData.SexType = sexType;
        }

        public void SetAvatarId(int avatarId)
        {
            _userData.AvatarId = avatarId;
        }

        public int GetAvatarId()
        {
            return _userData.AvatarId;
        }

        public void SetRulesType(RulesType rulesType)
        {
            _userData.SetRulesType(rulesType);
        }

        public void SetMoralityType(MoralityType moralityType)
        {
            _userData.SetMoralityType(moralityType);
        }

        public RulesType GetRulesType()
        {
            return _userData.GetRulesType();
        }

        public MoralityType GetMoralityType()
        {
            return _userData.GetMoralityType();
        }

        public RaceType GetRaceType()
        {
            return _userData.RaceType;
        }

        public void SetRaceType(RaceType raceType)
        {
            _userData.RaceType = raceType;
        }

        public int GetPropertyCount()
        {
            return _userData.PropertyCount;
        }

        public int GetSpiritualCount()
        {
            return _userData.SpiritualCount;
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
            return _propertyData.GetBaseProperty(basePropertyType) + GetInitBaseProperty(basePropertyType);
        }

        public int GetSpiritual(SpiritualType spiritual)
        {
            return _propertyData.GetSpiritual(spiritual) + GetInitSpiritual(spiritual);
        }

        public static int GetInitSpiritual(SpiritualType spiritualId)
        {
            var spiritual = GameEntry.DataTable.GetDataTable<DRSpiritual>();

            return spiritual.GetDataRow((int)spiritualId).InitValue;
        }

        public int GetMartialArts(MartialArtsType martialArtsType)
        {
            return _propertyData.GetMartialArts(martialArtsType) + GetInitMartialArts(martialArtsType);
        }

        public static int GetInitMartialArts(MartialArtsType martialArtsType)
        {
            var martialArts = GameEntry.DataTable.GetDataTable<DRMartialArts>();

            return martialArts.GetDataRow((int)martialArtsType).InitValue;
        }

        public int GetTechnique(TechniqueType techniqueType)
        {
            return _propertyData.GetTechnique(techniqueType) + GetInitTechnique(techniqueType);
        }

        public static int GetInitTechnique(TechniqueType techniqueType)
        {
            var technique = GameEntry.DataTable.GetDataTable<DRTechnique>();

            return technique.GetDataRow((int)techniqueType).InitValue;
        }

        public void AddTechnique(int techniqueId)
        {
            _userData.ReduceTechnique();
            _propertyData.AddTechnique(techniqueId);
        }

        public void ReduceTechnique(int techniqueId)
        {
            _userData.AddTechnique();
            _propertyData.ReduceTechnique(techniqueId);
        }

        public void AddMartialArts(int martialArtsId)
        {
            _userData.ReduceMartialArts();
            _propertyData.AddMartialArts(martialArtsId);
        }

        public void ReduceMartialArts(int martialArtsId)
        {
            _userData.AddMartialArts();
            _propertyData.ReduceMartialArts(martialArtsId);
        }

        public void AddBaseProperty(int propertyId)
        {
            _userData.ReduceProperty();
            _propertyData.AddBaseProperty(propertyId);
        }

        public void ReduceBaseProperty(int propertyId)
        {
            _userData.AddProperty();
            _propertyData.ReduceBaseProperty(propertyId);
        }

        public void AddSpiritual(int spiritualId)
        {
            _userData.ReduceSpiritual();
            _propertyData.AddSpiritual(spiritualId);
        }

        public void ReduceSpiritual(int spiritualId)
        {
            _userData.AddSpiritual();
            _propertyData.ReduceSpiritual(spiritualId);
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
            return _userData.MartialArtsCount;
        }

        public int GetTechniqueCount()
        {
            return _userData.TechniqueCount;
        }

        public bool HasTechnique()
        {
            var techniqueTable = GameEntry.DataTable.GetDataTable<DRTechnique>();

            return (from row in techniqueTable.GetAllDataRows() let technique = GetTechnique((TechniqueType)row.Id) where row.Beginner <= technique select row).Any();
        }

        public bool HasSelectedTalent(int talentId)
        {
            return _userData.HasSelectedTalent(talentId);
        }

        public void AddTalent(int id)
        {
            _userData.AddTalent(id);
        }

        public bool HasTalent(int id)
        {
            return _userData.HasTalent(id);
        }

        public bool CanAddTalent(int id)
        {
            return _userData.CanAddTalent(id);
        }

        public bool HasSelectTalent()
        {
            return _userData.HasSelectTalent();
        }

        public int GetSaveIndex()
        {
            return _userData.SaveIndex;
        }

        public void SetSaveIndex(int index)
        {
            _userData.SaveIndex = index;
        }

        public GameDifficultyType GetGameDifficultyType()
        {
            return _userData.GameDifficultyType;
        }

        public string GetFullName()
        {
            var surname = GameEntry.DataTable.GetDataTable<DRSurname>();

            return GameEntry.Localization.GetString(surname.GetDataRow(_userData.Surname).Name) + _userData.Name;
        }

        public string GetName()
        {
            return _userData.Name;
        }

        public int GetSurname()
        {
            return _userData.Surname;
        }

        public void RemoveTalent(int id)
        {
            _userData.RemoveTalent(id);
        }

        public void SetName(string playerName)
        {
            _userData.Name = playerName;
        }

        public void SetSurname(int surname)
        {
            _userData.Surname = surname;
        }

        public bool IsInitWorld()
        {
            return _userData.InitWorld;
        }

        public void SetInitWorld()
        {
            _userData.InitWorld = true;
        }

        public PropertyData GetPropertyData()
        {
            return _propertyData;
        }

        public UserData GetUserData()
        {
            return _userData;
        }

        public void Init(UserData initUserData, PropertyData initPropertyData)
        {
            _userData = initUserData;
            _propertyData = initPropertyData;
        }

        public void SetFamilyId(long id)
        {
            _userData.FamilyId = id;
        }

        public void SetSect(long id)
        {
            _userData.SectId = id;
        }

        public long GetFamilyId()
        {
            return _userData.FamilyId;
        }

        public long GetSectId()
        {
            return _userData.SectId;
        }
    }
}