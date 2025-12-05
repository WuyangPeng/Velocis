using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class TechniqueDisplay : MonoBehaviour
    {
        [SerializeField] 
        private Radio2Item[] items;

        [SerializeField] 
        private Text remainingText;

        public void Refresh()
        {
            var technique = GameEntry.DataTable.GetDataTable<DRTechnique>();
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            var index = 0;
            foreach (var element in technique)
            {
                var currentTechnique = userModule.GetTechnique((TechniqueType)element.Id);
                items[index].SetNum(element.Name, GetTechniqueName(currentTechnique, element), userModule.GetTechnique((TechniqueType)element.Id));

                ++index;
            }

            remainingText.text = userModule.GetTechniqueCount().ToString();
        }

        private static string GetTechniqueName(int currentTechnique, DRTechnique technique)
        {
            if (currentTechnique < technique.Beginner)
            {
                return "";
            }

            if (technique.Legendary <= currentTechnique)
            {
                return "Technique.Legendary";
            }

            if (technique.Grandmaster <= currentTechnique)
            {
                return "Technique.Grandmaster";
            }

            if (technique.Master <= currentTechnique)
            {
                return "Technique.Master";
            }

            return technique.Proficient <= currentTechnique ? "Technique.Proficient" : "Technique.Beginner";
        }
    }
}