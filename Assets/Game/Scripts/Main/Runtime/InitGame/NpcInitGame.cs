using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameEnum;
// using Game.Scripts.Main.Runtime.GameModule.User;
// using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.RuntimeException;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class NpcInitGame : InitGameBase
    {
        private readonly Dictionary<SexType, WeightRandom<int>> _avatarWeightRandom = new();
        private readonly WeightRandom<int> _campWeightRandom = new();
        private readonly Dictionary<int, HashSet<int>> _existName = new();
        // private readonly FamilyModule _familyModule = GameEntry.ModuleComponent.GetModule<FamilyModule>();
        // private readonly MapModule _mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();
        private readonly Dictionary<SexType, WeightRandom<int>> _nameWeightRandom = new();
        // private readonly NpcModule _npcModule = GameEntry.ModuleComponent.GetModule<NpcModule>();
        private readonly WeightRandom<int> _raceWeightRandom = new();
        // private readonly SectModule _sectModule = GameEntry.ModuleComponent.GetModule<SectModule>();
        private readonly WeightRandom<int> _surnameWeightRandom = new();
        private readonly WeightRandom<int> _talentWeightRandom = new();
        // private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        private void InitAvatar()
        {
            var avatarTable = GameEntry.DataTable.GetDataTable<DRAvatar>();
            var maleWeightRandom = new WeightRandom<int>();
            var femaleWeightRandom = new WeightRandom<int>();
            foreach (var element in avatarTable)
            {
                if ((element.Sex & (int)SexType.Male) != 0)
                {
                    maleWeightRandom.Add(element.Id, element.Weight);
                }

                if ((element.Sex & (int)SexType.Female) != 0)
                {
                    femaleWeightRandom.Add(element.Id, element.Weight);
                }
            }

            _avatarWeightRandom.Add(SexType.Male, maleWeightRandom);
            _avatarWeightRandom.Add(SexType.Female, femaleWeightRandom);
        }

        public override void InitGame()
        {
            InitExistName();
            InitAvatar();
            InitCamp();
            InitRace();
            InitTalent();
            InitSurname();
            InitName();
            InitFamily();
            InitNpc();
            InitNpcSect();
            InitNpcMap();
        }

        private void InitName()
        {
            var nameTable = GameEntry.DataTable.GetDataTable<DRName>();
            var maleWeightRandom = new WeightRandom<int>();
            var femaleWeightRandom = new WeightRandom<int>();
            foreach (var element in nameTable)
            {
                if ((element.Sex & (int)SexType.Male) != 0)
                {
                    maleWeightRandom.Add(element.Id, element.Weight);
                }

                if ((element.Sex & (int)SexType.Female) != 0)
                {
                    femaleWeightRandom.Add(element.Id, element.Weight);
                }
            }

            _nameWeightRandom.Add(SexType.Male, maleWeightRandom);
            _nameWeightRandom.Add(SexType.Female, femaleWeightRandom);
        }

        private void InitExistName()
        {
            // var surname = _userModule.GetSurname();
            // var name = _userModule.GetName();
            // if (!_existName.TryGetValue(surname, out var result))
            // {
            //     result = new HashSet<int>();
            //     _existName[surname] = result;
            // }

            // var nameTable = GameEntry.DataTable.GetDataTable<DRName>();
            // foreach (var element in nameTable)
            // {
            //     if (GameEntry.Localization.GetString(element.Name) != name)
            //     {
            //         continue;
            //     }

            //     result.Add(element.Id);
            //     break;
            // }
        }

        private void AddExistName(int surname, int name)
        {
            if (!_existName.TryGetValue(surname, out var result))
            {
                result = new HashSet<int>();
                _existName[surname] = result;
            }

            result.Add(name);
        }

        private bool IsExistName(int surname, int name)
        {
            return _existName.TryGetValue(surname, out var result) && result.Contains(name);
        }

        private void InitSurname()
        {
            var surnameTable = GameEntry.DataTable.GetDataTable<DRSurname>();
            foreach (var element in surnameTable)
            {
                _surnameWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitTalent()
        {
            var talentTable = GameEntry.DataTable.GetDataTable<DRTalent>();

            foreach (var element in talentTable)
            {
                _talentWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitRace()
        {
            var raceTable = GameEntry.DataTable.GetDataTable<DRRace>();

            foreach (var element in raceTable)
            {
                _raceWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitCamp()
        {
            var campTable = GameEntry.DataTable.GetDataTable<DRCamp>();

            foreach (var element in campTable)
            {
                if ((element.Group & (int)MoralityType.Empty) != 0 && (element.Group & (int)RulesType.Empty) != 0)
                {
                    _campWeightRandom.Add(element.Id, element.Weight);
                }
            }
        }

        private void InitFamily()
        {
            // var initNpcCount = _userModule.GetInitNpcCount();
            // foreach (var element in _familyModule.GetFamilies())
            // {
            //     for (var i = 0; i < Constant.Game.FamilyNpcRandomCount; ++i)
            //     {
            //         var sexType = GetSexType();
            //         var npcBaseData = new NpcBaseData
            //         {
            //             ID = _npcModule.GetNextNpcId(),
            //             SexType = sexType,
            //             AvatarId = GetAvatarId(sexType),
            //             CampType = (CampType)((_campWeightRandom.Roll() - (int)RulesType.Empty) | (element.MoralityType - MoralityType.Empty)),
            //             RaceType = element.RaceType,
            //             Surname = element.Surname,
            //             Name = GetName(element.Surname, sexType),
            //             FamilyId = element.ID
            //         };

            //         AddExistName(npcBaseData.Surname, npcBaseData.Name);
            //         npcBaseData.Talent.UnionWith(_talentWeightRandom.RollMultiple(Constant.Game.MaxTalentCount));

            //         _npcModule.AddNpc(npcBaseData);
            //         _mapModule.SetChunkByFamilyId(npcBaseData.ID, element.ID);

            //         if (_npcModule.GetNpcCount() > initNpcCount)
            //         {
            //             break;
            //         }
            //     }
            // }
        }

        private int GetName(int surname, SexType sexType)
        {
            if (_nameWeightRandom.TryGetValue(sexType, out var value))
            {
                if (value.Count == 0)
                {
                    var nameTable = GameEntry.DataTable.GetDataTable<DRName>();

                    foreach (var element in nameTable)
                    {
                        if ((element.Sex & (int)sexType) != 0)
                        {
                            value.Add(element.Id, element.Weight);
                        }
                    }
                }

                var name = value.Roll();
                if (!IsExistName(surname, name))
                {
                    value.Remove(name);
                    return name;
                }

                {
                    var weightRandom = new WeightRandom<int>();
                    var nameTable = GameEntry.DataTable.GetDataTable<DRName>();

                    if (!_existName.TryGetValue(surname, out var result))
                    {
                        result = new HashSet<int>();
                    }

                    foreach (var element in nameTable)
                    {
                        if ((element.Sex & (int)sexType) != 0 && !result.Contains(element.Id))
                        {
                            weightRandom.Add(element.Id, element.Weight);
                        }
                    }

                    name = weightRandom.Roll();
                    value.Remove(name);
                    return name;
                }
            }

            throw new GameException($"SexType {sexType} is not exist.");
        }

        private void InitNpc()
        {
            // var initNpcCount = _userModule.GetInitNpcCount();
            // for (var i = _npcModule.GetNpcCount(); i < initNpcCount; ++i)
            // {
            //     var sexType = GetSexType();
            //     var surname = _surnameWeightRandom.Roll();
            //     var npcBaseData = new NpcBaseData
            //     {
            //         ID = _npcModule.GetNextNpcId(),
            //         SexType = sexType,
            //         AvatarId = GetAvatarId(sexType),
            //         CampType = (CampType)_campWeightRandom.Roll(),
            //         RaceType = (RaceType)_raceWeightRandom.Roll(),
            //         Surname = surname,
            //         Name = GetName(surname, sexType)
            //     };

            //     AddExistName(npcBaseData.Surname, npcBaseData.Name);
            //     npcBaseData.Talent.UnionWith(_talentWeightRandom.RollMultiple(Constant.Game.MaxTalentCount));

            //     _npcModule.AddNpc(npcBaseData);
            // }
        }

        private int GetAvatarId(SexType sexType)
        {
            return _avatarWeightRandom.TryGetValue(sexType, out var value) ? value.Roll() : throw new GameException($"SexType {sexType} is not exist.");
        }

        private static SexType GetSexType()
        {
            return 0.5 <= Random.Range(0.0f, 1.0f) ? SexType.Female : SexType.Male;
        }

        private void InitNpcSect()
        {
            // WeightRandom<long> npcWeightRandom = new();
            // npcWeightRandom.Add(Constant.Game.PlayerId, 1);
            // foreach (var element in _npcModule.GetNpc())
            // {
            //     npcWeightRandom.Add(element.ID, 1);
            // }

            // foreach (var element in _sectModule.GetSects())
            // {
            //     for (var i = 0; i < Constant.Game.SectNpcRandomCount; i++)
            //     {
            //         if (npcWeightRandom.Count == 0)
            //         {
            //             return;
            //         }

            //         var id = npcWeightRandom.Roll();
            //         if (id == Constant.Game.PlayerId)
            //         {
            //             _userModule.SetSect(element.ID);
            //         }
            //         else
            //         {
            //             var npcBaseData = _npcModule.GetNpcBaseData(id);
            //             if (npcBaseData != null)
            //             {
            //                 npcBaseData.SectId = element.ID;
            //                 _mapModule.SetChunkBySectId(npcBaseData.ID, npcBaseData.SectId);
            //             }
            //         }

            //         npcWeightRandom.Remove(id);
            //     }
            // }
        }

        private void InitNpcMap()
        {
            // foreach (var element in _npcModule.GetNpc().Where(element => !_mapModule.HasEntity(element.ID)))
            // {
            //     _mapModule.AddEntityToRandomChunk(element.ID);
            // }
        }

        public override void SaveGame()
        {
            // var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "NpcData.idx");
            // var npcSaveData = new NpcSaveData
            // {
            //     Data = _npcModule.GetNpcData()
            // };

            // var json = Utility.Json.ToJson(npcSaveData);

            // fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}