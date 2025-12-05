using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class SectLoadGame : LoadGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly SectModule sectModule = GameEntry.ModuleComponent.GetModule<SectModule>();
        public override void LoadGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "SectData.idx");
            var bytes = fileSystems?.ReadFile("GameSaves");

            if (bytes == null)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(bytes);
            var data = Utility.Json.ToObject<SectSaveData>(json);

            sectModule.Init(data.Data);
        }
    }
}