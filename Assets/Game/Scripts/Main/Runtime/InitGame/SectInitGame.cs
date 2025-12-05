using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using GameFramework;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class SectInitGame : InitGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly SectModule sectModule = GameEntry.ModuleComponent.GetModule<SectModule>();
        private readonly MapModule mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();
        private readonly WeightRandom<int> moralityWeightRandom = new();
        private readonly WeightRandom<int> sectWeightRandom = new();
        public override void InitGame()
        {
            InitMorality();
            InitSect();
            DoInitGame();
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

        private void InitSect()
        {
            var sectTable = GameEntry.DataTable.GetDataTable<DRSect>();

            foreach (var element in sectTable)
            {
                sectWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void DoInitGame()
        {
            for (var i = 0; i < userModule.GetInitSectCount() - 1; i++)
            {
                if (sectWeightRandom.Count == 0)
                {
                    InitSect();
                }

                var sectBaseData = new SectBaseData
                {
                    ID = sectModule.GetNextSectId(),
                    MoralityType = (MoralityType)moralityWeightRandom.Roll(), 
                    SectId = sectWeightRandom.Roll()
                };

                sectModule.AddSect(sectBaseData);
                mapModule.AddSectToRandomChunk(sectBaseData);

                sectWeightRandom.Remove(sectBaseData.SectId);
            }
        }

        public override void SaveGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "SectData.idx");
            var sectSaveData = new SectSaveData
            {
                Data = sectModule.GetSectData()
            };

            var json = Utility.Json.ToJson(sectSaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}