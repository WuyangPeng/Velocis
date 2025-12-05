using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;
using System.Text;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class MapInitGame : InitGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly MapModule mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();

        private DRResourceLevel GetResourceLevel()
        {
            var gameDifficulty = userModule.GetGameDifficultyType();

            var gameDifficultyTable = GameEntry.DataTable.GetDataTable<DRGameDifficulty>();
            var resourceLevelTable = GameEntry.DataTable.GetDataTable<DRResourceLevel>();

            var gameDifficultyRow = gameDifficultyTable.GetDataRow((int)gameDifficulty);
            return resourceLevelTable.GetDataRow(gameDifficultyRow.ResourceLevel);
        }

        public override void InitGame()
        {
            var resourceLevel = GetResourceLevel();
            var initMapSize = userModule.GetInitMapSize();

            mapModule.SetMapSize(initMapSize);

            var weightRandom = new WeightRandom<int>();
            for (var i = 0; i < resourceLevel.LevelCount; ++i)
            {
                weightRandom.Add(i + 1, resourceLevel.GetLevel(i));
            }

            for (var x = 0; x < initMapSize; ++x)
            {
                for (var y = 0; y < initMapSize; ++y)
                {
                    var mapChunkData = new MapChunkData(x, y, weightRandom.Roll());

                    mapModule.AddMapChunkData(mapChunkData);
                }
            }
        }

        public override void SaveGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "MapData.idx");
            var mapSaveData = new MapSaveData
            {
                Data = mapModule.GetMapData()
            };

            var json = Utility.Json.ToJson(mapSaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}