using System.Text;
using Game.Scripts.Main.Runtime.Base;
// using Game.Scripts.Main.Runtime.GameModule.User;
// using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class SectLoadGame : LoadGameBase
    {
        // private readonly SectModule _sectModule = GameEntry.ModuleComponent.GetModule<SectModule>();
        // private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void LoadGame()
        {
            // var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "SectData.idx");
            // var bytes = fileSystems?.ReadFile("GameSaves");

            // if (bytes == null)
            // {
            //     return;
            // }

            // var json = Encoding.UTF8.GetString(bytes);
            // var data = Utility.Json.ToObject<SectSaveData>(json);

            // _sectModule.Init(data.Data);
        }
    }
}