using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class PropertyDisplay : MonoBehaviour
    {
        [SerializeField]
        private Radio2Item[] items;

        [SerializeField]
        private Text remainingText;

        public void Refresh()
        {
            var property = GameEntry.DataTable.GetDataTable<DRProperty>();
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            var index = 0;
            foreach (var element in property)
            {
                if (element.Group != (int)PropertyGroupType.Base) continue;


                items[index].SetNum(element.Name, userModule.GetBaseProperty((BasePropertyType)element.Id));

                ++index;
            }

            remainingText.text = userModule.GetPropertyCount().ToString();
        }
    }
}