using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using Game.Scripts.Main.Runtime.UIDisplay.UIHome;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public enum AvatarFormTab
    {
        Avatar = 0,
        Frame = 1,
        Title = 2
    }

    public class AvatarForm : UGuiForm
    {
        [SerializeField] private AvatarScrollDisplay avatarScrollDisplay;
        [SerializeField] private FrameScrollDisplay frameScrollDisplay;
        [SerializeField] private TitleScrollDisplay titleScrollDisplay;

        [SerializeField] private GameObject avatarButton;
        [SerializeField] private GameObject frameButton;
        [SerializeField] private GameObject titleButton;
        
        private AvatarFormTab _currentTab = AvatarFormTab.Avatar;
        private ProcedureHome _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open AvatarForm.");
            }

            SwitchTab(AvatarFormTab.Avatar);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureHome.RemoveUIForm(UIFormId.AvatarForm);
        }

        public void OnAvatarButtonClick()
        {
            SwitchTab(AvatarFormTab.Avatar);
        }

        public void OnFrameButtonClick()
        {
            SwitchTab(AvatarFormTab.Frame);
        }

        public void OnTitleButtonClick()
        {
            SwitchTab(AvatarFormTab.Title);
        }

        private void SwitchTab(AvatarFormTab tab)
        {
            _currentTab = tab;


            if (avatarScrollDisplay != null)
            {
                avatarScrollDisplay.gameObject.SetActive(tab == AvatarFormTab.Avatar);
            }

            if (frameScrollDisplay != null)
            {
                frameScrollDisplay.gameObject.SetActive(tab == AvatarFormTab.Frame);
            }

            if (titleScrollDisplay != null)
            {
                titleScrollDisplay.gameObject.SetActive(tab == AvatarFormTab.Title);
            }


            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            if (avatarButton != null)
            {
                var img = avatarButton.GetComponent<Image>();
                if (img != null)
                {
                    img.color = _currentTab == AvatarFormTab.Avatar ? Color.yellow : Color.white;
                }
            }

            if (frameButton != null)
            {
                var img = frameButton.GetComponent<Image>();
                if (img != null)
                {
                    img.color = _currentTab == AvatarFormTab.Frame ? Color.yellow : Color.white;
                }
            }

            if (titleButton != null)
            {
                var img = titleButton.GetComponent<Image>();
                if (img != null)
                {
                    img.color = _currentTab == AvatarFormTab.Title ? Color.yellow : Color.white;
                }
            }
        }
    }
}