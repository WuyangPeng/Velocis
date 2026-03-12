using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public class ProcedureCheckResources : ProcedureBase
    {
        private bool _checkResourcesComplete;
        private bool _needUpdateResources;
        private int _updateResourceCount;
        private long _updateResourceTotalCompressedLength;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _checkResourcesComplete = false;
            _needUpdateResources = false;
            _updateResourceCount = 0;
            _updateResourceTotalCompressedLength = 0L;

            GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!_checkResourcesComplete)
            {
                return;
            }

            if (_needUpdateResources)
            {
                procedureOwner.SetData<VarInt32>("UpdateResourceCount", _updateResourceCount);
                procedureOwner.SetData<VarInt64>("UpdateResourceTotalCompressedLength", _updateResourceTotalCompressedLength);
                ChangeState<ProcedureUpdateResources>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
        }

        private void OnCheckResourcesComplete(int movedCount, int removedCount, int updateCount, long updateTotalLength, long updateTotalCompressedLength)
        {
            _checkResourcesComplete = true;
            _needUpdateResources = updateCount > 0;
            _updateResourceCount = updateCount;
            _updateResourceTotalCompressedLength = updateTotalCompressedLength;
            Log.Info("Check resources complete, '{0}' resources need to update, compressed length is '{1}', uncompressed length is '{2}'.", updateCount.ToString(), updateTotalCompressedLength.ToString(), updateTotalLength.ToString());
        }
    }
}