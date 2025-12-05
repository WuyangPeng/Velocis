using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.UI;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure
{
    public class ProcedureUpdateResources : ProcedureBase
    {
        private bool m_UpdateResourcesComplete = false;
        private int m_UpdateCount = 0;
        private long m_UpdateTotalCompressedLength = 0L;
        private int m_UpdateSuccessCount = 0;
        private readonly List<UpdateLengthData> m_UpdateLengthData = new List<UpdateLengthData>();
        private UpdateResourceForm m_UpdateResourceForm = null;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_UpdateResourcesComplete = false;
            m_UpdateCount = procedureOwner.GetData<VarInt32>("UpdateResourceCount");
            procedureOwner.RemoveData("UpdateResourceCount");
            m_UpdateTotalCompressedLength = procedureOwner.GetData<VarInt64>("UpdateResourceTotalCompressedLength");
            procedureOwner.RemoveData("UpdateResourceTotalCompressedLength");
            m_UpdateSuccessCount = 0;
            m_UpdateLengthData.Clear();
            m_UpdateResourceForm = null;

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
                    OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });

                return;
            }

            StartUpdateResources(null);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_UpdateResourceForm != null)
            {
                Object.Destroy(m_UpdateResourceForm.gameObject);
                m_UpdateResourceForm = null;
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

            if (!m_UpdateResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private void StartUpdateResources(object userData)
        {
            if (m_UpdateResourceForm == null)
            {
                m_UpdateResourceForm = Object.Instantiate(GameEntry.BuiltinData.UpdateResourceFormTemplate);
            }

            Log.Info("Start update resources...");
            GameEntry.Resource.UpdateResources(OnUpdateResourcesComplete);
        }

        private void RefreshProgress()
        {
            var currentTotalUpdateLength = m_UpdateLengthData.Aggregate(0L, (current, data) => current + data.Length);

            var progressTotal = (float)currentTotalUpdateLength / m_UpdateTotalCompressedLength;
            var descriptionText = GameEntry.Localization.GetString("UpdateResource.Tips", m_UpdateSuccessCount.ToString(), m_UpdateCount.ToString(), GetByteLengthString(currentTotalUpdateLength), GetByteLengthString(m_UpdateTotalCompressedLength), progressTotal, GetByteLengthString((int)GameEntry.Download.CurrentSpeed));
            m_UpdateResourceForm.SetProgress(progressTotal, descriptionText);
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

        private void OnUpdateResourcesComplete(GameFramework.Resource.IResourceGroup resourceGroup, bool result)
        {
            if (result)
            {
                m_UpdateResourcesComplete = true;
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

            foreach (var data in m_UpdateLengthData.Where(data => data.Name == ne.Name))
            {
                Log.Warning("Update resource '{0}' is invalid.", ne.Name);
                data.Length = 0;
                RefreshProgress();
                return;
            }

            m_UpdateLengthData.Add(new UpdateLengthData(ne.Name));
        }

        private void OnResourceUpdateChanged(object sender, GameEventArgs e)
        {
            var ne = (ResourceUpdateChangedEventArgs)e;

            foreach (var data in m_UpdateLengthData.Where(data => data.Name == ne.Name))
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

            foreach (var data in m_UpdateLengthData.Where(data => data.Name == ne.Name))
            {
                data.Length = ne.CompressedLength;
                m_UpdateSuccessCount++;
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
            else
            {
                Log.Info("Update resource '{0}' failure from '{1}' with error message '{2}', retry count '{3}'.", ne.Name, ne.DownloadUri, ne.ErrorMessage, ne.RetryCount.ToString());
            }

            for (var i = 0; i < m_UpdateLengthData.Count; i++)
            {
                if (m_UpdateLengthData[i].Name != ne.Name) continue;
                m_UpdateLengthData.Remove(m_UpdateLengthData[i]);
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

            public int Length
            {
                get;
                set;
            }
        }
    }
}
