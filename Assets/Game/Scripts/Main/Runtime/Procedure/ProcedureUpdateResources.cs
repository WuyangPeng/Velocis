using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using ResourceUpdateChangedEventArgs = UnityGameFramework.Runtime.ResourceUpdateChangedEventArgs;
using ResourceUpdateFailureEventArgs = UnityGameFramework.Runtime.ResourceUpdateFailureEventArgs;
using ResourceUpdateStartEventArgs = UnityGameFramework.Runtime.ResourceUpdateStartEventArgs;
using ResourceUpdateSuccessEventArgs = UnityGameFramework.Runtime.ResourceUpdateSuccessEventArgs;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public class ProcedureUpdateResources : ProcedureBase
    {
        private readonly List<UpdateLengthData> _updateLengthData = new();
        private int _updateCount;
        private UpdateResourceForm _updateResourceForm;
        private bool _updateResourcesComplete;
        private int _updateSuccessCount;
        private long _updateTotalCompressedLength;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _updateResourcesComplete = false;
            _updateCount = procedureOwner.GetData<VarInt32>("UpdateResourceCount");
            procedureOwner.RemoveData("UpdateResourceCount");
            _updateTotalCompressedLength = procedureOwner.GetData<VarInt64>("UpdateResourceTotalCompressedLength");
            procedureOwner.RemoveData("UpdateResourceTotalCompressedLength");
            _updateSuccessCount = 0;
            _updateLengthData.Clear();
            _updateResourceForm = null;

            GameEntry.Event.Subscribe(ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
            GameEntry.Event.Subscribe(ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
            GameEntry.Event.Subscribe(ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
            GameEntry.Event.Subscribe(ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 2,
                    Title = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.Title"),
                    Message = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.Message"),
                    ConfirmText = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.UpdateButton"),
                    OnClickConfirm = StartUpdateResources,
                    CancelText = GameEntry.Localization.GetString("UpdateResourceViaCarrierDataNetwork.QuitButton"),
                    OnClickCancel = delegate { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); }
                });

                return;
            }

            StartUpdateResources(null);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (_updateResourceForm != null)
            {
                Object.Destroy(_updateResourceForm.gameObject);
                _updateResourceForm = null;
            }

            GameEntry.Event.Unsubscribe(ResourceUpdateStartEventArgs.EventId, OnResourceUpdateStart);
            GameEntry.Event.Unsubscribe(ResourceUpdateChangedEventArgs.EventId, OnResourceUpdateChanged);
            GameEntry.Event.Unsubscribe(ResourceUpdateSuccessEventArgs.EventId, OnResourceUpdateSuccess);
            GameEntry.Event.Unsubscribe(ResourceUpdateFailureEventArgs.EventId, OnResourceUpdateFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!_updateResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private void StartUpdateResources(object userData)
        {
            if (_updateResourceForm == null)
            {
                _updateResourceForm = Object.Instantiate(GameEntry.BuiltinData.UpdateResourceFormTemplate);
            }

            Log.Info("Start update resources...");
            GameEntry.Resource.UpdateResources(OnUpdateResourcesComplete);
        }

        private void RefreshProgress()
        {
            var currentTotalUpdateLength = _updateLengthData.Aggregate(0L, (current, data) => current + data.Length);

            var progressTotal = (float)currentTotalUpdateLength / _updateTotalCompressedLength;
            var descriptionText = GameEntry.Localization.GetString("UpdateResource.Tips", _updateSuccessCount.ToString(), _updateCount.ToString(), GetByteLengthString(currentTotalUpdateLength), GetByteLengthString(_updateTotalCompressedLength), progressTotal, GetByteLengthString((int)GameEntry.Download.CurrentSpeed));
            _updateResourceForm.SetProgress(progressTotal, descriptionText);
        }

        private static string GetByteLengthString(long byteLength)
        {
            return byteLength switch
            {
                // 2 ^ 10
                < 1024L => Utility.Text.Format("{0} Bytes", byteLength),
                // 2 ^ 20
                < 1048576L => Utility.Text.Format("{0:F2} KB", byteLength / 1024f),
                // 2 ^ 30
                < 1073741824L => Utility.Text.Format("{0:F2} MB", byteLength / 1048576f),
                // 2 ^ 40
                < 1099511627776L => Utility.Text.Format("{0:F2} GB", byteLength / 1073741824f),
                // 2 ^ 50
                < 1125899906842624L => Utility.Text.Format("{0:F2} TB", byteLength / 1099511627776f),
                // 2 ^ 60
                < 1152921504606846976L => Utility.Text.Format("{0:F2} PB", byteLength / 1125899906842624f),
                _ => Utility.Text.Format("{0:F2} EB", byteLength / 1152921504606846976f)
            };
        }

        private void OnUpdateResourcesComplete(IResourceGroup resourceGroup, bool result)
        {
            if (result)
            {
                _updateResourcesComplete = true;
                Log.Info("Update resources complete with no errors.");
            }
            else
            {
                Log.Error("Update resources complete with errors.");
            }
        }

        private void OnResourceUpdateStart(object sender, GameEventArgs e)
        {
            var ne = (ResourceUpdateStartEventArgs)e;

            foreach (var data in _updateLengthData.Where(data => data.Name == ne.Name))
            {
                Log.Warning("Update resource '{0}' is invalid.", ne.Name);
                data.Length = 0;
                RefreshProgress();
                return;
            }

            _updateLengthData.Add(new UpdateLengthData(ne.Name));
        }

        private void OnResourceUpdateChanged(object sender, GameEventArgs e)
        {
            var ne = (ResourceUpdateChangedEventArgs)e;

            foreach (var data in _updateLengthData.Where(data => data.Name == ne.Name))
            {
                data.Length = ne.CurrentLength;
                RefreshProgress();
                return;
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private void OnResourceUpdateSuccess(object sender, GameEventArgs e)
        {
            var ne = (ResourceUpdateSuccessEventArgs)e;
            Log.Info("Update resource '{0}' success.", ne.Name);

            foreach (var data in _updateLengthData.Where(data => data.Name == ne.Name))
            {
                data.Length = ne.CompressedLength;
                _updateSuccessCount++;
                RefreshProgress();
                return;
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private void OnResourceUpdateFailure(object sender, GameEventArgs e)
        {
            var ne = (ResourceUpdateFailureEventArgs)e;
            if (ne.RetryCount >= ne.TotalRetryCount)
            {
                Log.Error("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
                return;
            }

            Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());

            for (var i = 0; i < _updateLengthData.Count; i++)
            {
                if (_updateLengthData[i].Name != ne.Name)
                {
                    continue;
                }

                _updateLengthData.Remove(_updateLengthData[i]);
                RefreshProgress();
                return;
            }

            Log.Warning("Update resource '{0}' is invalid.", ne.Name);
        }

        private class UpdateLengthData
        {
            public UpdateLengthData(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public int Length { get; set; }
        }
    }
}