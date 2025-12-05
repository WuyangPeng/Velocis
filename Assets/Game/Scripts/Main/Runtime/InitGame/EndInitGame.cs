using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User; 
using GameFramework;
using System.Text;
using Game.Scripts.Main.Runtime.SaveData;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class EndInitGame : InitGameBase
    {

        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void InitGame()
        {
   
        }

        public override void SaveGame()
        {
            userModule.SetInitWorld();

            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "UserData.idx");
            var userSaveData = new UserSavaData
            {
                UserData = userModule.GetUserData(),
                PropertyData = userModule.GetPropertyData()
            };

            var json = Utility.Json.ToJson(userSaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}