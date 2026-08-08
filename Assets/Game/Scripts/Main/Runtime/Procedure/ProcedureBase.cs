using System;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
    {
        public abstract bool UseNativeDialog { get; }

        protected ProcedureOwner m_ProcedureOwner;

        public virtual ProcedureOwner ProcedureOwner => m_ProcedureOwner;

        public void ChangeStateByType(ProcedureOwner fsm, Type stateType)
        {
            ChangeState(fsm, stateType);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_ProcedureOwner = procedureOwner;
        }
    }
}
