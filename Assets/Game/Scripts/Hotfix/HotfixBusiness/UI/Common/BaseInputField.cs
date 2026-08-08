using Game.Scripts.Main.Runtime.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    [RequireComponent(typeof(TMP_InputField))]
    public class BaseInputField : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] [UISoundId] private int clickSound;
        [SerializeField] [UISoundId] private int typeSound;

        private TMP_InputField _inputField;
        private int _lastTextLength;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _lastTextLength = _inputField.text.Length;
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            if (_inputField != null)
            {
                _inputField.onValueChanged.RemoveListener(OnValueChanged);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (clickSound > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(clickSound);
            }
        }

        private void OnValueChanged(string newText)
        {
            if (newText.Length > _lastTextLength && typeSound > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(typeSound);
            }

            _lastTextLength = newText.Length;
        }
    }
}
