using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class SpiritualDisplay : MonoBehaviour
    {
        [SerializeField] 
        private Radio2Item[] items;

        [SerializeField] 
        private Text remainingText;

        public void Refresh()
        {
            var spiritual = GameEntry.DataTable.GetDataTable<DRSpiritual>();
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            var index = 0;
            foreach (var element in spiritual)
            {
                var currentSpiritual = userModule.GetSpiritual((SpiritualType)element.Id);
                items[index].SetNum(element.Name, GetSpiritualName(currentSpiritual, element), userModule.GetSpiritual((SpiritualType)element.Id));

                ++index;
            }

            remainingText.text = userModule.GetSpiritualCount().ToString();
        }

        private static string GetSpiritualName(int currentSpiritual, DRSpiritual spiritual)
        {
            if (currentSpiritual < spiritual.EnableValue)
            {
                return "";
            }

            if (spiritual.Innate <= currentSpiritual)
            {
                return "Spiritual.Innate";
            }

            if (spiritual.BestQuality <= currentSpiritual)
            {
                return "Spiritual.BestQuality";
            }

            if (spiritual.TopGrade <= currentSpiritual)
            {
                return "Spiritual.TopGrade";
            }

            return spiritual.QualityProduct <= currentSpiritual ? "Spiritual.QualityProduct" : "Spiritual.EnableValue";
        }
    }
}