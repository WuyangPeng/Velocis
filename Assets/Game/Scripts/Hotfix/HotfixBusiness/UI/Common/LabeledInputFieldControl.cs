// 创建时间：2026-07-26
// 修改时间：2026-07-26

using TMPro;
using UnityEngine;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    /// <summary>
    /// 包含文本标签与 TMP_InputField 输入框的组合 UI 控件组件。
    /// </summary>
    public class LabeledInputFieldControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TMP_InputField inputField;

        public TextMeshProUGUI LabelText => labelText;
        public TMP_InputField InputField => inputField;
    }
}
