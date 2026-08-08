using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class HelpItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;

        public TMP_Text TitleText => titleText;
        public TMP_Text DescriptionText => descriptionText;
        public Image IconImage => iconImage;
    }
}
