// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using System;
using Game.Scripts.Hotfix.HotfixBusiness.Procedure.Scene;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameUtility;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.Sound;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Hotfix.HotfixBusiness.Procedure
{
    /// <summary>
    /// 切换场景流程。
    /// </summary>
    public class ProcedureChangeScene : ProcedureBase
    {
        /// <summary>
        /// 背景音乐 ID。
        /// </summary>
        private int _backgroundMusicId;

        /// <summary>
        /// 场景切换是否已完成。
        /// </summary>
        private bool _isChangeSceneComplete;

        /// <summary>
        /// 场景类型。
        /// </summary>
        private SceneType _sceneType = SceneType.Start;

        /// <summary>
        /// 获取是否使用原生对话框。
        /// </summary>
        public override bool UseNativeDialog => false;

        /// <summary>
        /// 流程进入时的初始化逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _isChangeSceneComplete = false;

            GameEntryEnter();

            var loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            foreach (var sceneName in loadedSceneAssetNames)
            {
                GameEntry.Scene.UnloadScene(sceneName);
            }

            GameEntry.Base.ResetNormalGameSpeed();

            LoadScene(procedureOwner);
        }

        /// <summary>
        /// 开始加载场景。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        private void LoadScene(ProcedureOwner procedureOwner)
        {
            int sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
            _sceneType = (SceneType)sceneId;
            var dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            var drScene = dtScene.GetDataRow(sceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }

            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
            _backgroundMusicId = drScene.BackgroundMusicId;
        }

        /// <summary>
        /// 进入流程时初始化事件监听及清理声音和实体。
        /// </summary>
        private void GameEntryEnter()
        {
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();
        }

        /// <summary>
        /// 流程离开时的清理逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否正在关闭游戏框架。</param>
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            Unsubscribe();

            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 取消订阅事件。
        /// </summary>
        private void Unsubscribe()
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
        }

        /// <summary>
        /// 流程轮询更新逻辑。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，单位秒。</param>
        /// <param name="realElapseSeconds">真实流逝时间，单位秒。</param>
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!_isChangeSceneComplete)
            {
                return;
            }

            ChangeScene(procedureOwner);
        }

        /// <summary>
        /// 场景加载完成后的状态切换。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        private void ChangeScene(ProcedureOwner procedureOwner)
        {
            switch (_sceneType)
            {
                case SceneType.Start:
                    ChangeState<ProcedureStart>(procedureOwner);
                    break;
                case SceneType.Create:
                    ChangeState<ProcedureCreate>(procedureOwner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 加载场景成功事件回调。
        /// </summary>
        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            if (_backgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(_backgroundMusicId);
            }

            _isChangeSceneComplete = true;
        }

        /// <summary>
        /// 加载场景失败事件回调。
        /// </summary>
        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        /// <summary>
        /// 加载场景进度更新事件回调。
        /// </summary>
        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        /// <summary>
        /// 加载场景依赖资源事件回调。
        /// </summary>
        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            var ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
