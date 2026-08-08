using Game.Scripts.Main.Runtime.GameEnum;
// using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectGameDifficultyForm : UGuiForm
    {
        [SerializeField] private GameDifficultyDisplay gameDifficultyDisplay;

        private IProcedureCreateHost _procedureCreate;

        public void OnReturnButtonClick()
        {
            _procedureCreate.ReturnMenu();
        }

        public void OnEnterButtonClick(int gameDifficulty)
        {
            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetGameDifficulty((GameDifficultyType)gameDifficulty);

            _procedureCreate.OpenUIForm(UIFormId.SelectGameParameterForm);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureCreate = (IProcedureCreateHost)GetCurrentProcedure();

            if (_procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectGameDifficultyForm.");
            }

            gameDifficultyDisplay.Refresh();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }
    }
}