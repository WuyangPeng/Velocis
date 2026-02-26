using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class BottomForm : UGuiForm
    {
        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open BottomForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnHeroButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.HeroForm);
        }

        public void OnSkillButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.SkillForm);
        }

        public void OnMainCityButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.MainCityForm);
        }


        public void OnCopyButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.CopyForm);
        }

        public void OnBackpackButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.BackpackForm);
        }
    }
}