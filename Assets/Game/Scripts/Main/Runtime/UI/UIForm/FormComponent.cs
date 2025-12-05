using System.Collections.Generic;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Event;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.UI.UIForm
{
    public class FormComponent
    {

        private readonly Dictionary<UIFormId, UGuiForm> uGuiForm = new();

        public void OnEnter(ProcedureOwner procedureOwner)
        {
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            foreach (var form in uGuiForm)
            {
                GameEntry.UI.OpenUIForm(form.Key, new FormComponentUserData(this, form.Key));
            }
        }

        public void OpenUIForm(UIFormId form)
        {
            AddForm(form);
            GameEntry.UI.OpenUIForm(form, new FormComponentUserData(this, form));
        }

        public void AddForm(UIFormId form)
        {
            uGuiForm.Add(form, null);
        }

        public void RemoveUIForm(UIFormId form)
        {
            var ui = uGuiForm[form];
            if (ui != null)
            {
                ui.Close();
            }

            uGuiForm.Remove(form);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            var ne = (UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs)e;

            if (ne.UserData is not FormComponentUserData userData)
            {
                return;
            }

            if (userData.FormComponent != this)
            {
                return;
            }

            uGuiForm[userData.FormId] = (UGuiForm)ne.UIForm.Logic;
        }

        public void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            foreach (var form in uGuiForm)
            {
                form.Value.Close(true);
            }

            uGuiForm.Clear();
        }
    }
}