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
    public class SelectSpiritualForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private SpiritualDisplay spiritualDisplay;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectSpiritualForm.");
            }

            spiritualDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectSpiritualForm);
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

                    procedureCreate.OpenUIForm(UIFormId.SelectMartialArtsForm);
                },
            });
        }

        public void OnEnterButtonClick()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (!userModule.HasSpiritual())
            {
                OpenDialog("Spiritual.OpenSpiritual.Title", "Spiritual.OpenSpiritual.Content");
                return;
            }

            if (0 < userModule.GetSpiritualCount())
            {
                OpenDialog("Spiritual.Allocate.Title", "Spiritual.Allocate.Content");
                return;
            }

            procedureCreate.OpenUIForm(UIFormId.SelectMartialArtsForm);
        }

        public void OnReduceButtonClick(int spiritualId)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var spiritual = userModule.GetSpiritual((SpiritualType)spiritualId);
            var initSpiritual = UserModule.GetInitSpiritual((SpiritualType)spiritualId);
            if (spiritual <= initSpiritual || userModule.GetSpiritualCount() >= Constant.Game.InitSpiritualCount)
            {
                return;
            }

            userModule.ReduceSpiritual(spiritualId);
            spiritualDisplay.Refresh();
        }

        public void OnAddButtonClick(int spiritualId)
        {
            var spiritualTable = GameEntry.DataTable.GetDataTable<DRSpiritual>();

            var row = spiritualTable.GetDataRow(spiritualId);
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var spiritual = userModule.GetSpiritual((SpiritualType)spiritualId);
            if (spiritual >= row.MaxValue || userModule.GetSpiritualCount() <= 0)
            {
                return;
            }

            userModule.AddSpiritual(spiritualId);
            spiritualDisplay.Refresh();
        }

    }
}