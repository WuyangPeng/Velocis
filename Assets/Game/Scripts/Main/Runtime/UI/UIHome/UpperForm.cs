using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class UpperForm : UGuiForm
    {
        [SerializeField] private AvatarItem avatarItem;

        private ProcedureHome _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open UpperForm.");
            }

            var avatarModule = GameEntry.ModuleComponent.GetModule<AvatarModule>();
            var selectedAvatar = avatarModule.GetSelectedAvatar();
            if (selectedAvatar == null)
            {
                return;
            }

            var avatarConfig = GameEntry.GameConfig.GetGameConfig().GetTables().AvatarConfigContainer.Get(selectedAvatar.Inventory.TemplateId);
            if (avatarConfig != null)
            {
                avatarItem.SetSprite(avatarConfig.IconRes);
            }

            var frameModule = GameEntry.ModuleComponent.GetModule<FrameModule>();
            var selectedFrame = frameModule.GetSelectedFrame();
            if (selectedFrame == null)
            {
                return;
            }

            var frameConfig = GameEntry.GameConfig.GetGameConfig().GetTables().FrameConfigContainer.Get(selectedFrame.Inventory.TemplateId);
            if (frameConfig != null)
            {
                avatarItem.SetFrameSprite(frameConfig.IconRes);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnPersonalInformationButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.PersonalInformationForm);
        }
    }
}