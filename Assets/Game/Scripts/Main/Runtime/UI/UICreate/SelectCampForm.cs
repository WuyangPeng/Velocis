using System.Collections.Generic;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameEnum;
// using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectCampForm : UGuiForm
    {
        [SerializeField] private CampDisplay campDisplay;

        [SerializeField] private Toggle[] rulesToggle;

        [SerializeField] private Toggle[] moralityToggle;

        [SerializeField] private InputField inputField;

        [SerializeField] private Dropdown surnameDropdown;


        private readonly List<DRSurname> _surnames = new();
        private IProcedureCreateHost _procedureCreate;

        public void OnClickConfirm()
        {
            // var playerName = inputField.text;
            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetName(playerName);
        }

        private void OnNameSelected(int index)
        {
            // var chosen = _surnames[index].Id;
            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetSurname(chosen);
        }

        public void OnLeftRulesButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetRulesType(RulesType.Lawful);
        }

        public void OnMiddleRulesButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetRulesType(RulesType.Carefree);
        }

        public void OnRightRulesButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetRulesType(RulesType.Chaos);
        }


        public void OnLeftMoralityButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetMoralityType(MoralityType.Benevolence);
        }

        public void OnMiddleMoralityButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetMoralityType(MoralityType.Moderation);
        }

        public void OnRightMoralityButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            // userModule.SetMoralityType(MoralityType.Craftiness);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureCreate = (IProcedureCreateHost)GetCurrentProcedure();

            if (_procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectCampForm.");
            }

            campDisplay.Refresh();

            InitCamp();
            InitDropdown();
            InitInputField();
        }

        private void InitInputField()
        {
            // inputField.text = GameEntry.ModuleComponent.GetModule<UserModule>().GetName();
        }

        private void InitDropdown()
        {
            var surname = GameEntry.DataTable.GetDataTable<DRSurname>();
            foreach (var element in surname)
            {
                var optionData = new Dropdown.OptionData
                {
                    text = GameEntry.Localization.GetString(element.Name)
                };

                surnameDropdown.options.Add(optionData);

                _surnames.Add(element);
            }

            surnameDropdown.onValueChanged.AddListener(OnNameSelected);

            surnameDropdown.value = 0;
        }

        private void InitCamp()
        {
            // var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            // var rulesType = (int)userModule.GetRulesType() & (int)MoralityType.Empty;

            // for (var i = 0; i < rulesToggle.Length; i++)
            // {
            //     if ((rulesType & (1 << i)) != 0)
            //     {
            //         rulesToggle[i].isOn = true;
            //     }
            // }

            // var moralityType = (int)userModule.GetMoralityType() >> 3;

            // for (var i = 0; i < moralityToggle.Length; i++)
            // {
            //     if ((moralityType & (1 << i)) != 0)
            //     {
            //         moralityToggle[i].isOn = true;
            //     }
            // }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureCreate.RemoveUIForm(UIFormId.SelectCampForm);
        }

        public void OnEnterButtonClick()
        {
            _procedureCreate.OpenUIForm(UIFormId.SelectRaceForm);
        }
    }
}