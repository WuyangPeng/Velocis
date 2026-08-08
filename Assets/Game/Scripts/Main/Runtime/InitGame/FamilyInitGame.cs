using System.Text;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameEnum;
// using Game.Scripts.Main.Runtime.GameModule.User;
// using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class FamilyInitGame : InitGameBase
    {
        // private readonly FamilyModule _familyModule = GameEntry.ModuleComponent.GetModule<FamilyModule>();
        // private readonly MapModule _mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();
        private readonly WeightRandom<int> _moralityWeightRandom = new();
        private readonly WeightRandom<int> _raceWeightRandom = new();
        private readonly WeightRandom<int> _surnameWeightRandom = new();
        // private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void InitGame()
        {
            InitMorality();
            InitRace();
            InitSurname();
            InitPlayerFamily();
            InitOtherFamily();
        }

        private void InitMorality()
        {
            var campTable = GameEntry.DataTable.GetDataTable<DRCamp>();

            foreach (var element in campTable)
            {
                if (element.Group == (int)MoralityType.Empty)
                {
                    _moralityWeightRandom.Add(element.Id, element.Weight);
                }
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

        private void InitSurname()
        {
            var surnameTable = GameEntry.DataTable.GetDataTable<DRSurname>();

            foreach (var element in surnameTable)
            {
                _surnameWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitPlayerFamily()
        {
            // var familyBaseData = new FamilyBaseData
            // {
            //     ID = _familyModule.GetNextFamilyId(),
            //     MoralityType = _userModule.GetMoralityType(),
            //     RaceType = _userModule.GetRaceType(),
            //     Surname = _userModule.GetSurname()
            // };

            // _familyModule.AddFamily(familyBaseData);
            // _mapModule.AddFamilyToRandomChunk(familyBaseData);

            // _surnameWeightRandom.Remove(familyBaseData.Surname);
            // _userModule.SetFamilyId(familyBaseData.ID);
            // _mapModule.SetChunkByFamilyId(Constant.Game.PlayerId, familyBaseData.ID);
        }

        private void InitOtherFamily()
        {
            // for (var i = 0; i < _userModule.GetInitFamilyCount() - 1; i++)
            // {
            //     if (_surnameWeightRandom.Count == 0)
            //     {
            //         InitSurname();
            //     }

            //     var familyBaseData = new FamilyBaseData
            //     {
            //         ID = _familyModule.GetNextFamilyId(),
            //         MoralityType = (MoralityType)_moralityWeightRandom.Roll(),
            //         RaceType = (RaceType)_raceWeightRandom.Roll(),
            //         Surname = _surnameWeightRandom.Roll()
            //     };

            //     _familyModule.AddFamily(familyBaseData);
            //     _mapModule.AddFamilyToRandomChunk(familyBaseData);

            //     _surnameWeightRandom.Remove(familyBaseData.Surname);
            // }
        }

        public override void SaveGame()
        {
            // var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "FamilyData.idx");
            // var familySaveData = new FamilySaveData
            // {
            //     CurrentFamilyId = _familyModule.GetCurrentFamilyId(),
            //     FamilyBaseDataContainer = _familyModule.GetFamilies()
            // };

            // var json = Utility.Json.ToJson(familySaveData);

            // fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}