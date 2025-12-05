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
    public class SelectMartialArtsForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private MartialArtsDisplay martialArtsDisplay;



        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectMartialArtsForm.");
            }

            martialArtsDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectMartialArtsForm);
        }

        public void OnEnterButtonClick()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (!userModule.HasMartialArts())
            {
                OpenDialog("MartialArts.OpenMartialArts.Title", "MartialArts.OpenMartialArts.Content");
                return;
            }

            if (0 < userModule.GetMartialArtsCount())
            {
                OpenDialog("MartialArts.Allocate.Title", "MartialArts.Allocate.Content");
                return;
            }

            procedureCreate.OpenUIForm(UIFormId.SelectTechniqueForm);
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

                    procedureCreate.OpenUIForm(UIFormId.SelectTechniqueForm);
                },
            });
        }

        public void OnReduceButtonClick(int martialArtsId)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var martialArts = userModule.GetMartialArts((MartialArtsType)martialArtsId);
            var initMartialArts = UserModule.GetInitMartialArts((MartialArtsType)martialArtsId);
            if (martialArts <= initMartialArts || userModule.GetMartialArtsCount() >= Constant.Game.InitMartialArtsCount)
            {
                return;
            }

            userModule.ReduceMartialArts(martialArtsId);
            martialArtsDisplay.Refresh();
        }

        public void OnAddButtonClick(int martialArtsId)
        {
            var martialArtsTable = GameEntry.DataTable.GetDataTable<DRMartialArts>();

            var row = martialArtsTable.GetDataRow(martialArtsId);
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var martialArts = userModule.GetMartialArts((MartialArtsType)martialArtsId);
            if (martialArts >= row.MaxValue || userModule.GetMartialArtsCount() <= 0)
            {
                return;
            }

            userModule.AddMartialArts(martialArtsId);
            martialArtsDisplay.Refresh();
        }
    }
}