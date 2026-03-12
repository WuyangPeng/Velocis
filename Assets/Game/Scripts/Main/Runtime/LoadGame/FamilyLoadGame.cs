using System.Text;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class FamilyLoadGame : LoadGameBase
    {
        private readonly FamilyModule _familyModule = GameEntry.ModuleComponent.GetModule<FamilyModule>();
        private readonly UserModule _userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

        public override void LoadGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + _userModule.GetSaveIndex(), "FamilyData.idx");
            var bytes = fileSystems?.ReadFile("GameSaves");

            if (bytes == null)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(bytes);
            var data = Utility.Json.ToObject<FamilySaveData>(json);

            _familyModule.Init(data.CurrentFamilyId, data.FamilyBaseDataContainer);
        }
    }
}