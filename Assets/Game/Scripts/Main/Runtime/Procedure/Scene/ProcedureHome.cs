using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureHome : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;
        private readonly FormComponent formComponent = new();

        private readonly Dictionary<GameMode, GameBase> m_Games = new();

        private GameBase m_CurrentGame;

        //private bool m_GotoMenu;
        private float m_GotoMenuDelaySeconds;

        public override bool UseNativeDialog => false;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // m_GotoMenu = false;
            var gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();

            formComponent.AddForm(UIFormId.BottomForm);
            formComponent.AddForm(UIFormId.UpperForm);
            formComponent.AddForm(UIFormId.LeftForm);
            formComponent.AddForm(UIFormId.RightForm);

            formComponent.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ServerListEventArgs.EventId, OnServerListClose);
        }

        private void OnServerListClose(object sender, GameEventArgs e)
        {
            RemoveUIForm(UIFormId.ServerListForm);
        }

        public void OpenUIForm(UIFormId form)
        {
            formComponent.OpenUIForm(form);
        }

        public void RemoveUIForm(UIFormId formId)
        {
            formComponent.RemoveUIForm(formId);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ServerListEventArgs.EventId, OnServerListClose);

            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);

            formComponent.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            if (m_CurrentGame is { GameOver: false })
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
            }

            /* if (!m_GotoMenu)
             {
                 m_GotoMenu = true;
                 m_GotoMenuDelaySeconds = 0;
             }

             m_GotoMenuDelaySeconds += elapseSeconds;
             if (!(m_GotoMenuDelaySeconds >= GameOverDelayedSeconds))
             {
                 return;
             }

             procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
             ChangeState<ProcedureChangeScene>(procedureOwner);*/
        }
    }
}