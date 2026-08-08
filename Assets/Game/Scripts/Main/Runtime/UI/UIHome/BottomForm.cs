using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class BottomForm : UGuiForm
    {
        private IProcedureFormHost _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open BottomForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnHeroButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.HeroForm);
        }

        public void OnSkillButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.SkillForm);
        }

        public void OnMainCityButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.MainCityForm);
        }


        public void OnCopyButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.CopyForm);
        }

        public void OnBackpackButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.BackpackForm);
        }
    }
}