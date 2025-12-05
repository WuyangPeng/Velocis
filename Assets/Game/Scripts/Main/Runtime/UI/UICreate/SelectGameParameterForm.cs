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
    public class SelectGameParameterForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private GameParameterDisplay gameParameterDisplay;

        [SerializeField]
        private Toggle[] mapSizeToggle;

        [SerializeField]
        private Toggle[] npcCountToggle;

        [SerializeField]
        private Toggle[] sectCountToggle;

        [SerializeField]
        private Toggle[] familyCountToggle;

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectGameParameterForm);
        }

        public void OnEnterButtonClick()
        {
            procedureCreate.OpenUIForm(UIFormId.SelectAvatarForm);
        }

        public void OnSmallMapSizeButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetMapSize(GameParameterType.Small);
        }

        public void OnMiddleMapSizeButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetMapSize(GameParameterType.Middle);
        }

        public void OnBigMapSizeButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetMapSize(GameParameterType.Big);
        }

        public void OnSmallNpcCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetNpcCount(GameParameterType.Small);
        }

        public void OnMiddleNpcCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetNpcCount(GameParameterType.Middle);
        }


        public void OnBigNpcCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetNpcCount(GameParameterType.Big);
        }

        public void OnSmallSectCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetSectCount(GameParameterType.Small);
        }

        public void OnMiddleSectCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetSectCount(GameParameterType.Middle);
        }
        public void OnBigSectCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetSectCount(GameParameterType.Big);
        }

        public void OnSmallFamilyCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetFamilyCount(GameParameterType.Small);
        }

        public void OnMiddleFamilyCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetFamilyCount(GameParameterType.Middle);
        }

        public void OnBigFamilyCountButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetFamilyCount(GameParameterType.Big);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectGameParameterForm.");
            }

            gameParameterDisplay.Refresh();

            InitGameParameter();
        }

        private void InitGameParameter()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            var gameParameter = GameEntry.DataTable.GetDataTable<DRGameParameter>();
            var rows = gameParameter.GetAllDataRows();
            var initMapSize = userModule.GetInitMapSize();
            var initNpcCount = userModule.GetInitNpcCount();
            var initSectCount = userModule.GetInitSectCount();
            var initFamilyCount = userModule.GetInitFamilyCount();

            for (var index = 0; index < rows.Length; ++index)
            {
                SetToggleOn(rows, index, initMapSize, initNpcCount, initSectCount, initFamilyCount);
            }
        }

        private void SetToggleOn(DRGameParameter[] rows, int index, int initMapSize, int initNpcCount, int initSectCount, int initFamilyCount)
        {
            var row = rows[index];
            if (row.MinMapSize <= initMapSize && initMapSize <= row.MaxMapSize)
            {
                mapSizeToggle[index].isOn = true;
            }

            if (row.MinNpcCount <= initNpcCount && initNpcCount <= row.MaxNpcCount)
            {
                npcCountToggle[index].isOn = true;
            }

            if (row.MinSectCount <= initSectCount && initSectCount <= row.MaxSectCount)
            {
                sectCountToggle[index].isOn = true;
            }

            if (row.MinFamilyCount <= initFamilyCount && initFamilyCount <= row.MaxFamilyCount)
            {
                familyCountToggle[index].isOn = true;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }
    }
}