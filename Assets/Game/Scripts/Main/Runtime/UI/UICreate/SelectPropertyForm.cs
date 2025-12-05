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
    public class SelectPropertyForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private PropertyDisplay propertyDisplay;

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectPropertyForm);
        }

        public void OnEnterButtonClick()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (0 < userModule.GetPropertyCount())
            {
                GameEntry.UI.OpenDialog(new DialogParams()
                {
                    Mode = 2,
                    Title = GameEntry.Localization.GetString("Property.Allocate.Title"),
                    Message = GameEntry.Localization.GetString("Property.Allocate.Content"),
                    OnClickConfirm = delegate (object userData) {
                        GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(UIFormId.DialogForm));
                        
                        procedureCreate.OpenUIForm(UIFormId.SelectSpiritualForm); },
                });
                return;
            }


            procedureCreate.OpenUIForm(UIFormId.SelectSpiritualForm);
        }

        public void OnReduceButtonClick(int propertyId)
        {
            var property = GameEntry.DataTable.GetDataTable<DRProperty>();

            var row = property.GetDataRow(propertyId);
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var baseProperty = userModule.GetBaseProperty((BasePropertyType)propertyId);
            var initBaseProperty = userModule.GetInitBaseProperty((BasePropertyType)propertyId);
            if (baseProperty <= initBaseProperty || userModule.GetPropertyCount() >= Constant.Game.InitPropertyCount)
            {
                return;
            }

            userModule.ReduceBaseProperty(propertyId);
            propertyDisplay.Refresh();
        }

        public void OnAddButtonClick(int propertyId)
        {
            var property = GameEntry.DataTable.GetDataTable<DRProperty>();

            var row = property.GetDataRow(propertyId);
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var baseProperty = userModule.GetBaseProperty((BasePropertyType)propertyId);
            if (baseProperty >= row.MaxValue || userModule.GetPropertyCount() <= 0)
            {
                return;
            }

            userModule.AddBaseProperty(propertyId);
            propertyDisplay.Refresh();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectPropertyForm.");
            }

            propertyDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }
    }
}