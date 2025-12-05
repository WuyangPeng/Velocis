using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class MapLoadGame : LoadGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly MapModule mapModule = GameEntry.ModuleComponent.GetModule<MapModule>();

        public override void LoadGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "MapData.idx");
            var bytes = fileSystems?.ReadFile("GameSaves");

            if (bytes == null)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(bytes);
            var data = Utility.Json.ToObject<MapSaveData>(json);

            mapModule.Init(data.Data);
        }
    }
}