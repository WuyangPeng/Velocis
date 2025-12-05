using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectGameDifficultyForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private GameDifficultyDisplay gameDifficultyDisplay;

        public void OnReturnButtonClick()
        {
            procedureCreate.ReturnMenu();
        }

        public void OnEnterButtonClick(int gameDifficulty)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetGameDifficulty((GameDifficultyType)gameDifficulty);

            procedureCreate.OpenUIForm(UIFormId.SelectGameParameterForm);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectGameDifficultyForm.");
            }

            gameDifficultyDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }
    }
}