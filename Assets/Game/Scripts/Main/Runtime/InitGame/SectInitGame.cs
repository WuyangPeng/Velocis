using System.Text;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class SectInitGame : InitGameBase
    {
        private readonly MapModule _mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();
        private readonly WeightRandom<int> _moralityWeightRandom = new();
        private readonly SectModule _sectModule = GameEntry.ModuleComponent.GetModule<SectModule>();
        private readonly WeightRandom<int> _sectWeightRandom = new();
        private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

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
                    _moralityWeightRandom.Add(element.Id, element.Weight);
                }
            }
        }

        private void InitSect()
        {
            var sectTable = GameEntry.DataTable.GetDataTable<DRSect>();

            foreach (var element in sectTable)
            {
                _sectWeightRandom.Add(element.Id, element.Weight);
            }
        }

        private void DoInitGame()
        {
            for (var i = 0; i < _userModule.GetInitSectCount() - 1; i++)
            {
                if (_sectWeightRandom.Count == 0)
                {
                    InitSect();
                }

                var sectBaseData = new SectBaseData
                {
                    ID = _sectModule.GetNextSectId(),
                    MoralityType = (MoralityType)_moralityWeightRandom.Roll(),
                    SectId = _sectWeightRandom.Roll()
                };

                _sectModule.AddSect(sectBaseData);
                _mapModule.AddSectToRandomChunk(sectBaseData);

                _sectWeightRandom.Remove(sectBaseData.SectId);
            }
        }

        public override void SaveGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "SectData.idx");
            var sectSaveData = new SectSaveData
            {
                Data = _sectModule.GetSectData()
            };

            var json = Utility.Json.ToJson(sectSaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}