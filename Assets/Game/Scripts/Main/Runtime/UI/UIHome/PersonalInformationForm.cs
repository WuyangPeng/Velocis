// using Game.Scripts.Main.Runtime.Event;
// using Game.Scripts.Main.Runtime.GameModule.Item;
// using Game.Scripts.Main.Runtime.GameModule.Role;
// using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIItem.UIHome;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class PersonalInformationForm : UGuiForm
    {
        [SerializeField] private TMP_Text roleName;

        [SerializeField] private TMP_Text userId;

        [SerializeField] private TMP_Text serverName;

        [SerializeField] private TMP_Text version;

        [SerializeField] private AvatarItem avatarItem;

        [SerializeField] private TMP_Text title;

        private IProcedureFormHost _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open PersonalInformationForm.");
            }

            SetText();

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userId.text = userModule.GetUserId().ToString();

            // var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            // serverName.text = accountModule.GetCurrentGameServerName();
            version.text = "1.0.0";

            // GameEntry.Event.Subscribe(ChangeNameEventArgs.EventId, OnSetTextSuccess);

            // var avatarModule = GameEntry.ModuleComponent.GetModule<AvatarModule>();
            // var selectedAvatar = avatarModule.GetSelectedAvatar();
            // if (selectedAvatar == null)
            // {
            //     return;
            // }

//            var avatarConfig = GameEntry.GameConfig.GetGameConfig().GetTables().AvatarConfigContainer.Get(selectedAvatar.Inventory.TemplateId);
//            if (avatarConfig != null)
//            {
//                avatarItem.SetSprite(avatarConfig.IconRes);
//            }

            // var frameModule = GameEntry.ModuleComponent.GetModule<FrameModule>();
            // var selectedFrame = frameModule.GetSelectedFrame();
            // if (selectedFrame == null)
            // {
            //     return;
            // }

//            var frameConfig = GameEntry.GameConfig.GetGameConfig().GetTables().FrameConfigContainer.Get(selectedFrame.Inventory.TemplateId);
//            if (frameConfig != null)
//            {
//                avatarItem.SetFrameSprite(frameConfig.IconRes);
//            }


            // var titleModule = GameEntry.ModuleComponent.GetModule<TitleModule>();
            // var selectedTitle = titleModule.GetSelectedTitle();
            // if (selectedTitle == null)
            // {
            //     title.text = "";
            //     return;
            // }


//            var titleConfig = GameEntry.GameConfig.GetGameConfig().GetTables().TitleConfigContainer.Get(selectedTitle.Inventory.TemplateId);
//            if (titleConfig != null)
//            {
//                title.text = GameEntry.Localization.GetString(titleConfig.Text);
//            }
            // else
            {
                title.text = "";
            }
        }

        private void OnSetTextSuccess(object sender, GameEventArgs e)
        {
            SetText();
        }

        private void SetText()
        {
            // var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            // roleName.text = roleModule.GetFullName();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            // GameEntry.Event.Unsubscribe(ChangeNameEventArgs.EventId, OnSetTextSuccess);

            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureHome.RemoveUIForm(UIFormId.PersonalInformationForm);
        }

        public void OnSetButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnAvatarButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.AvatarForm);
        }

        public void OnChangeNameButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.ChangeNameForm);
        }

        public void OnServerListButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.ServerListForm);
        }
    }
}