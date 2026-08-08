using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Menu
{
    public class DialogForm : UGuiForm
    {
        [SerializeField] private TMP_Text titleText;

        [SerializeField] private TMP_Text messageText;

        [SerializeField] private GameObject[] modeObjects;

        [SerializeField] private TMP_Text[] confirmTexts;

        [SerializeField] private TMP_Text[] cancelTexts;

        [SerializeField] private TMP_Text[] otherTexts;

        private GameFrameworkAction<object> _onClickCancel;

        private GameFrameworkAction<object> _onClickConfirm;
        private GameFrameworkAction<object> _onClickOther;

        public int DialogMode { get; private set; } = 1;

        public bool PauseGame { get; private set; }

        public object UserData { get; private set; }

        public void OnConfirmButtonClick()
        {
            Close();

            _onClickConfirm?.Invoke(UserData);
        }

        public void OnCancelButtonClick()
        {
            Close();

            _onClickCancel?.Invoke(UserData);
        }

        public void OnOtherButtonClick()
        {
            Close();

            _onClickOther?.Invoke(UserData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var dialogParams = (DialogParams)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            titleText.text = dialogParams.Title;
            messageText.text = dialogParams.Message;

            PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            UserData = dialogParams.UserData;

            RefreshConfirmText(dialogParams.ConfirmText);
            _onClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            _onClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            _onClickOther = dialogParams.OnClickOther;
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            if (PauseGame)
            {
                GameEntry.Base.ResumeGame();
            }

            DialogMode = 1;
            titleText.text = string.Empty;
            messageText.text = string.Empty;
            PauseGame = false;
            UserData = null;

            RefreshConfirmText(string.Empty);
            _onClickConfirm = null;

            RefreshCancelText(string.Empty);
            _onClickCancel = null;

            RefreshOtherText(string.Empty);
            _onClickOther = null;

            base.OnClose(isShutdown, userData);
        }

        private void RefreshDialogMode()
        {
            for (var i = 1; i <= modeObjects.Length; i++)
            {
                modeObjects[i - 1].SetActive(i == DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton");
            }

            foreach (var text in confirmTexts)
            {
                text.text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameEntry.Localization.GetString("Dialog.CancelButton");
            }

            foreach (var text in cancelTexts)
            {
                text.text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameEntry.Localization.GetString("Dialog.OtherButton");
            }

            foreach (var text in otherTexts)
            {
                text.text = otherText;
            }
        }
    }
}