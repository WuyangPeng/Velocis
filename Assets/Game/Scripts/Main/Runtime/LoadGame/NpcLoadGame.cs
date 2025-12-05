using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class NpcLoadGame : LoadGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly NpcModule npcModule = GameEntry.ModuleComponent.GetModule<NpcModule>();
        public override void LoadGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "NpcData.idx");
            var bytes = fileSystems?.ReadFile("GameSaves");

            if (bytes == null)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(bytes);
            var data = Utility.Json.ToObject<NpcSaveData>(json);

            npcModule.Init(data.Data);
        }
    }
}