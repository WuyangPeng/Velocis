using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.GameModule.World;
using Game.Scripts.Main.Runtime.SaveData;
using System.Text;
using Game.Scripts.Main.Runtime.Base;
using GameFramework;

namespace Game.Scripts.Main.Runtime.LoadGame
{
    public class FamilyLoadGame : LoadGameBase
    {
        private readonly UserModule userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
        private readonly FamilyModule familyModule = GameEntry.ModuleComponent.GetModule<FamilyModule>();

        public override void LoadGame()
        {
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + userModule.GetSaveIndex(), "FamilyData.idx");
            var bytes = fileSystems?.ReadFile("GameSaves");

            if (bytes == null)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(bytes);
            var data = Utility.Json.ToObject<FamilySaveData>(json);

            familyModule.Init(data.CurrentFamilyId, data.FamilyBaseDataContainer);
        }
    }
}