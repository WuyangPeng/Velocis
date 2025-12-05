using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class DialogForm : UGuiForm
    {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Text messageText = null;

        [SerializeField]
        private GameObject[] modeObjects = null;

        [SerializeField]
        private Text[] confirmTexts = null;

        [SerializeField]
        private Text[] cancelTexts = null;

        [SerializeField]
        private Text[] otherTexts = null;

        private GameFrameworkAction<object> onClickConfirm = null;
        private GameFrameworkAction<object> onClickCancel = null;
        private GameFrameworkAction<object> onClickOther = null;

        public int DialogMode { get; private set; } = 1;

        public bool PauseGame { get; private set; }

        public object UserData { get; private set; }

        public void OnConfirmButtonClick()
        {
            Close();

            onClickConfirm?.Invoke(UserData);
        }

        public void OnCancelButtonClick()
        {
            Close();

            onClickCancel?.Invoke(UserData);
        }

        public void OnOtherButtonClick()
        {
            Close();

            onClickOther?.Invoke(UserData);
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
            onClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            onClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            onClickOther = dialogParams.OnClickOther;
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            if (PauseGame)
            {
                Base.GameEntry.Base.ResumeGame();
            }

            DialogMode = 1;
            titleText.text = string.Empty;
            messageText.text = string.Empty;
            PauseGame = false;
            UserData = null;

            RefreshConfirmText(string.Empty);
            onClickConfirm = null;

            RefreshCancelText(string.Empty);
            onClickCancel = null;

            RefreshOtherText(string.Empty);
            onClickOther = null;

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
                Base.GameEntry.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = Base.GameEntry.Localization.GetString("Dialog.ConfirmButton");
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
                cancelText = Base.GameEntry.Localization.GetString("Dialog.CancelButton");
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
                otherText = Base.GameEntry.Localization.GetString("Dialog.OtherButton");
            }

            foreach (var text in otherTexts)
            {
                text.text = otherText;
            }
        }
    }
}
