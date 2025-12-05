using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectTechniqueForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private TechniqueDisplay techniqueDisplay;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectTechniqueForm.");
            }

            techniqueDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectTechniqueForm);
        }

        private void OpenDialog(string title, string message)
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString(title),
                Message = GameEntry.Localization.GetString(message),
                OnClickConfirm = delegate (object userData)
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.DialogForm));

                    procedureCreate.OpenUIForm(UIFormId.SelectTalentForm);
                },
            });
        }

        public void OnEnterButtonClick()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (!userModule.HasTechnique())
            {
                OpenDialog("Technique.OpenTechnique.Title", "Technique.OpenTechnique.Content");
                return;
            }

            if (0 < userModule.GetTechniqueCount())
            {
                OpenDialog("Technique.Allocate.Title", "Technique.Allocate.Content");
                return;
            }

            procedureCreate.OpenUIForm(UIFormId.SelectTalentForm);
        }

        public void OnReduceButtonClick(int techniqueId)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var technique = userModule.GetTechnique((TechniqueType)techniqueId);
            var initTechnique = UserModule.GetInitTechnique((TechniqueType)techniqueId);
            if (technique <= initTechnique || userModule.GetTechniqueCount() >= Constant.Game.InitTechniqueCount)
            {
                return;
            }

            userModule.ReduceTechnique(techniqueId);
            techniqueDisplay.Refresh();
        }

        public void OnAddButtonClick(int techniqueId)
        {
            var techniqueTable = GameEntry.DataTable.GetDataTable<DRTechnique>();

            var row = techniqueTable.GetDataRow(techniqueId);
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var technique = userModule.GetTechnique((TechniqueType)techniqueId);
            if (technique >= row.MaxValue || userModule.GetTechniqueCount() <= 0)
            {
                return;
            }

            userModule.AddTechnique(techniqueId);
            techniqueDisplay.Refresh();
        }
    }
}