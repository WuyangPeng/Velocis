using System.Text;
using Game.Scripts.Main.Runtime.Base;
// using Game.Scripts.Main.Runtime.GameModule.User;
// using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class NpcLoadGame : LoadGameBase
    {
        // private readonly NpcModule _npcModule = GameEntry.ModuleComponent.GetModule<NpcModule>();
        // private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void LoadGame()
        {
            // var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "NpcData.idx");
            // var bytes = fileSystems?.ReadFile("GameSaves");

            // if (bytes == null)
            // {
            //     return;
            // }

            // var json = Encoding.UTF8.GetString(bytes);
            // var data = Utility.Json.ToObject<NpcSaveData>(json);

            // _npcModule.Init(data.Data);
        }
    }
}