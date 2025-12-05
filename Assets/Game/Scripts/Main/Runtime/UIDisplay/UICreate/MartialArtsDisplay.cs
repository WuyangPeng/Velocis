using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class MartialArtsDisplay : MonoBehaviour
    {
        [SerializeField] 
        private Radio2Item[] items;

        [SerializeField] 
        private Text remainingText;

        public void Refresh()
        {
            var martialArts = GameEntry.DataTable.GetDataTable<DRMartialArts>();
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            var index = 0;
            foreach (var element in martialArts)
            {
                var currentMartialArts = userModule.GetMartialArts((MartialArtsType)element.Id);
                items[index].SetNum(element.Name, GetMartialArtsName(currentMartialArts, element), userModule.GetMartialArts((MartialArtsType)element.Id));

                ++index;
            }

            remainingText.text = userModule.GetMartialArtsCount().ToString();
        }

        private static string GetMartialArtsName(int currentMartialArts, DRMartialArts martialArts)
        {
            if (currentMartialArts < martialArts.Beginner)
            {
                return "";
            }

            if (martialArts.Legendary <= currentMartialArts)
            {
                return "MartialArts.Legendary";
            }

            if (martialArts.Grandmaster <= currentMartialArts)
            {
                return "MartialArts.Grandmaster";
            }

            if (martialArts.Master <= currentMartialArts)
            {
                return "MartialArts.Master";
            }

            return martialArts.Proficient <= currentMartialArts ? "MartialArts.Proficient" : "MartialArts.Beginner";
        }
    }
}