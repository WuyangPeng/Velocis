using Game.Scripts.Main.Runtime.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIItem.UICreate
{
    public class Radio2Item : MonoBehaviour
    {
        [SerializeField] 
        private Text leftText;

        [SerializeField] 
        private Text rightText;

        public void SetData(string leftKey, string rightKey)
        {
            leftText.text = GameEntry.Localization.GetString(leftKey);
            rightText.text = GameEntry.Localization.GetString(rightKey);
        }

        public void SetNum(string key, int num)
        {
            leftText.text = GameEntry.Localization.GetString(key);
            rightText.text = num.ToString();
        }

        public void SetNum(string key, string suffixKey, int num)
        {
            if (suffixKey.Length == 0)
            {
                leftText.text = GameEntry.Localization.GetString(key);
            }
            else
            {
                leftText.text = GameEntry.Localization.GetString(key) + GameEntry.Localization.GetString(suffixKey);
            }

            rightText.text = num.ToString();
        }
    }
}