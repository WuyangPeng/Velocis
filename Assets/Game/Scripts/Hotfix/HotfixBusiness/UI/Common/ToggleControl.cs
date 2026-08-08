using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class ToggleControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Toggle toggle;

        public TextMeshProUGUI LabelText => labelText;
        public Toggle Toggle => toggle;
    }
}
