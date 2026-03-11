using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectRaceForm : UGuiForm
    {
        [SerializeField] private RaceDisplay raceDisplay;

        [SerializeField] private Text raceTitle;


        [SerializeField] private Text raceDescriptionTitle;

        private ProcedureCreate _procedureCreate;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (_procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectRaceForm.");
            }

            raceDisplay.Refresh();

            SetTitle();
        }

        private void SetTitle()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var raceType = userModule.GetRaceType();

            var races = GameEntry.DataTable.GetDataTable<DRRace>();

            var race = races.GetDataRow((int)raceType);

            raceTitle.text = GameEntry.Localization.GetString(race.Name);
            raceDescriptionTitle.text = GameEntry.Localization.GetString(race.Description);
        }

        public void OnSelectHumanButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetRaceType(RaceType.Human);

            SetTitle();
        }

        public void OnSelectDemonButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetRaceType(RaceType.Demon);

            SetTitle();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureCreate.RemoveUIForm(UIFormId.SelectRaceForm);
        }

        public void OnEnterButtonClick()
        {
            _procedureCreate.OpenUIForm(UIFormId.SelectPropertyForm);
        }
    }
}