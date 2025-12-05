using Game.Scripts.Main.Runtime.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIItem.UICreate
{
    public class Radio3Item : MonoBehaviour
    {
        [SerializeField] 
        private Text leftText;

        [SerializeField] 
        private Text middleText;

        [SerializeField] 
        private Text rightText;

        public void SetData(string leftKey, string middleKey, string rightKey)
        {
            leftText.text = GameEntry.Localization.GetString(leftKey);
            middleText.text = GameEntry.Localization.GetString(middleKey);
            rightText.text = GameEntry.Localization.GetString(rightKey);
        }
    }
}