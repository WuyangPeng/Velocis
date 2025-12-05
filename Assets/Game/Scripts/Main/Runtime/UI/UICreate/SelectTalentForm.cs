using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectTalentForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectTalentForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectTalentForm);
        }

        public void OnEnterButtonClick()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (!userModule.HasSelectTalent())
            {
                GameEntry.UI.OpenDialog(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Talent.OpenTalent.Title"),
                    Message = GameEntry.Localization.GetString("Talent.OpenTalent.Content")
                });
          
                return;
            }
      
            procedureCreate.SaveData();
            procedureCreate.EnterGame();
        }

    }
}