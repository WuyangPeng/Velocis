using System.Text;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.InitGame
{
    public class EndInitGame : InitGameBase
    {
        private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void InitGame()
        {
        }

        public override void SaveGame()
        {
            _userModule.SetInitWorld();

            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "UserData.idx");
            var userSaveData = new UserSavaData
            {
                UserData = _userModule.GetUserData(),
                PropertyData = _userModule.GetPropertyData()
            };

            var json = Utility.Json.ToJson(userSaveData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}