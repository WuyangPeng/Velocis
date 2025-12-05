using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using Game.Scripts.Main.Runtime.Definition.Constant;
using GameFramework;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class FamilyInitGame : InitGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly MapModule mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();
        private readonly FamilyModule familyModule = GameEntry.ModuleComponent.GetModule<FamilyModule>();
        private readonly WeightRandom<int> moralityWeightRandom = new();
        private readonly WeightRandom<int> raceWeightRandom = new();
        private readonly WeightRandom<int> surnameWeightRandom = new();

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
                    moralityWeightRandom.Add(element.Id, element.Weight);
                }
            }

        }
        private void InitRace()
        {
            var raceTable = GameEntry.DataTable.GetDataTable<DRRace>();

            foreach (var element in raceTable)
            {
                raceWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitSurname()
        {
            var surnameTable = GameEntry.DataTable.GetDataTable<DRSurname>();

            foreach (var element in surnameTable)
            {
                surnameWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void InitPlayerFamily()
        {
            var familyBaseData = new FamilyBaseData
            {
                ID = familyModule.GetNextFamilyId(),
                MoralityType = userModule.GetMoralityType(),
                RaceType = userModule.GetRaceType(),
                Surname = userModule.GetSurname(),
            };

            familyModule.AddFamily(familyBaseData);
            mapModule.AddFamilyToRandomChunk(familyBaseData);

            surnameWeightRandom.Remove(familyBaseData.Surname);
            userModule.SetFamilyId(familyBaseData.ID);
            mapModule.SetChunkByFamilyId(Constant.Game.PlayerId, familyBaseData.ID);
        }

        private void InitOtherFamily()
        {
            for (var i = 0; i < userModule.GetInitFamilyCount() - 1; i++)
            {
                if (surnameWeightRandom.Count == 0)
                {
                    InitSurname();
                }

                var familyBaseData = new FamilyBaseData
                {
                    ID = familyModule.GetNextFamilyId(),
                    MoralityType = (MoralityType)moralityWeightRandom.Roll(),
                    RaceType = (RaceType)raceWeightRandom.Roll(),
                    Surname = surnameWeightRandom.Roll()
                };

                familyModule.AddFamily(familyBaseData);
                mapModule.AddFamilyToRandomChunk(familyBaseData);

                surnameWeightRandom.Remove(familyBaseData.Surname);
            }
        }

        public override void SaveGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "FamilyData.idx");
            var familySaveData = new FamilySaveData
            {
                CurrentFamilyId = familyModule.GetCurrentFamilyId(),
                FamilyBaseDataContainer = familyModule.GetFamilies()
            };

            var json = Utility.Json.ToJson(familySaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}