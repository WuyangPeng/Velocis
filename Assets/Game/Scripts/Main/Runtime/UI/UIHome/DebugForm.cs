using System;
using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.GameModule.Debug;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;


namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class DebugForm : UGuiForm
    {
        [SerializeField] private InputField idInputField;
        [SerializeField] private InputField parameterInputField;
        [SerializeField] private Dropdown typeDropdown;

        private ProcedureHome _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open DebugForm.");
            }

            InitDropdown();
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureHome.RemoveUIForm(UIFormId.DebugForm);
        }

        private void InitDropdown()
        {
            for (var i = debug_type.AddItem; i > debug_type.None; --i)
            {
                var optionData = new Dropdown.OptionData
                {
                    text = i.ToString()
                };
                typeDropdown.options.Add(optionData);
            }


            typeDropdown.onValueChanged.AddListener(OnTypeSelected);

            typeDropdown.value = 0;
        }

        private void OnTypeSelected(int index)
        {
        }

        public void OnDebugButtonClick()
        {
            var debugModule = GameEntry.ModuleComponent.GetModule<DebugModule>();
            long id = 0;
            if (!string.IsNullOrEmpty(idInputField.text))
            {
                id = Convert.ToInt64(idInputField.text);
            }

            long parameter = 0;
            if (!string.IsNullOrEmpty(parameterInputField.text))
            {
                parameter = Convert.ToInt64(parameterInputField.text);
            }

            debugModule.SendDebugMessage((debug_type)typeDropdown.value, id, parameter);
        }
    }
}