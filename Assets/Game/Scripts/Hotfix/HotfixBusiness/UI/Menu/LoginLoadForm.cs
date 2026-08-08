// 创建时间：2026-07-31
// 修改时间：2026-08-03
// 审核时间：

using System.Collections;

using Game.Scripts.Hotfix.HotfixBusiness.Procedure.Scene;
using Game.Scripts.Hotfix.HotfixCommon.Config;
using Game.Scripts.Hotfix.HotfixCommon.Config.Processors;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using GameFramework.Event;
using GameFramework.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Menu
{
    public class LoginLoadForm : UGuiForm
    {
        private const float LerpSpeed = 5.0f; // 插值系数 k
        private const float MinShowTime = 1.0f; // 最小展示时间
        private const float TimeoutDuration = 8.0f; // 超时阈值

        [SerializeField] private Image bgImage;
        [SerializeField] private SliderControl progressSlider;
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private TMP_Text tipsText;
        [SerializeField] private CanvasGroup tipsCanvasGroup;
        [SerializeField] [SoundId] private int openSoundId;
        [SerializeField] [SoundId] private int finishSoundId;




        private float _currentProgress;
        private float _elapsedTime;
        private bool _isFinished;
        private ProcedureStart _procedureStart;
        private float _targetProgress;
        private float _timer;
        private Coroutine _tipsCoroutine;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureStart = GetCurrentProcedure() as ProcedureStart;
            if (_procedureStart == null)
            {
                Log.Warning("ProcedureStart is invalid when open LoginLoadForm.");
            }

            _currentProgress = 0f;
            _targetProgress = 0f;
            _elapsedTime = 0f;
            _timer = 0f;
            _isFinished = false;

            if (progressSlider != null && progressSlider.Slider != null)
            {
                progressSlider.Slider.value = 0f;
            }

            if (percentageText != null)
            {
                percentageText.text = string.Format(GameEntry.Localization.GetString("Loading.TextPrefix") ?? "调兵遣将中...", 0);
            }

            // 加载权重随机背景图
            var imageProcessor = GameEntry.GameConfig.GetProcessor<RandomSceneImageProcessor>();
            if (imageProcessor != null && bgImage != null)
            {
                var randomImage = imageProcessor.GetRandomImage();
                if (randomImage != null)
                {
                    GameEntry.Resource.LoadAsset(randomImage.ImageRes, typeof(Sprite), new LoadAssetCallbacks((assetName, asset, duration, userState) =>
                    {
                        if (bgImage != null && asset != null)
                        {
                            bgImage.sprite = asset as Sprite;
                        }
                    }));
                }
            }

            // 播放开场号角声
            GameEntry.Sound.PlaySound(openSoundId);

            // 订阅进度更新事件
            GameEntry.Event.Subscribe(LoginProgressEventArgs.EventId, OnLoginProgress);
            // 订阅网络关闭事件
            GameEntry.Event.Subscribe(NetworkCloseEventArgs.EventId, OnNetworkClosed);
            // 订阅网络请求错误事件
            GameEntry.Event.Subscribe(CeleritasErrorEventArgs.EventId, OnCeleritasError);

            // 异步加载显示首个权重随机提示语
            ShowRandomTip();
            if (tipsCanvasGroup != null)
            {
                tipsCanvasGroup.alpha = 1f;
            }

            // 使用 GameEntry.Base 启动协程，防止由于 UI GameObject 初始状态为 Inactive 导致协程启动失败
            _tipsCoroutine = GameEntry.Base.StartCoroutine(CoRotateTips());
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if (_tipsCoroutine != null && GameEntry.Base != null)
            {
                GameEntry.Base.StopCoroutine(_tipsCoroutine);
                _tipsCoroutine = null;
            }

            if (GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(LoginProgressEventArgs.EventId, OnLoginProgress);
                GameEntry.Event.Unsubscribe(NetworkCloseEventArgs.EventId, OnNetworkClosed);
                GameEntry.Event.Unsubscribe(CeleritasErrorEventArgs.EventId, OnCeleritasError);
            }

            _procedureStart = null;
            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            _elapsedTime += elapseSeconds;
            _timer += elapseSeconds;

            // 超时检测
            if (_timer >= TimeoutDuration && !_isFinished)
            {
                HandleTimeout();
                return;
            }

            // 进度插值推进
            var limit = _targetProgress < 1.0f ? 0.99f : 1.00f;
            if (_currentProgress < limit)
            {
                _currentProgress = Mathf.Lerp(_currentProgress, limit, LerpSpeed * elapseSeconds);
                if (limit - _currentProgress < 0.001f)
                {
                    _currentProgress = limit;
                }
            }
            else if (_targetProgress >= 1.0f && _currentProgress < 1.0f)
            {
                // 收到加载完毕信号，迅速滑向 1.00
                _currentProgress = Mathf.MoveTowards(_currentProgress, 1.0f, LerpSpeed * elapseSeconds);
            }

            if (progressSlider != null && progressSlider.Slider != null)
            {
                progressSlider.Slider.value = _currentProgress;
            }

            if (percentageText != null)
            {
                percentageText.text = string.Format("{0} {1}%", GameEntry.Localization.GetString("Loading.TextPrefix") ?? "调兵遣将中...", Mathf.FloorToInt(_currentProgress * 100f));
            }

            // 检查是否加载完成并达到了最小展示时间
            if (_currentProgress >= 1.0f && _elapsedTime >= MinShowTime && !_isFinished)
            {
                _isFinished = true;
                FinishLoading();
            }
        }

        private void OnLoginProgress(object sender, GameEventArgs e)
        {
            if (e is LoginProgressEventArgs args)
            {
                _targetProgress = args.Progress;
                // 每次收到进度更新，重置超时计时器
                _timer = 0f;
            }
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            if (e is not NetworkCloseEventArgs)
            {
                return;
            }
            
            Close(true);
        }

        private void OnCeleritasError(object sender, GameEventArgs e)
        {
            if (e is not CeleritasErrorEventArgs args)
            {
                return;
            }

            if (args.Rpc != NetworkChannelHelper.LoginRpcId)
            {
                return;
            }

            // 登录失败，且玩家已在通用错误框点击确定后，关闭连接（这会触发网络断开，进而关闭当前加载界面）
            var channel = GameEntry.Network.GetNetworkChannel(NetworkConstant.TcpChannel);
            if (channel is { Connected: true })
            {
                channel.Close();
            }
            else
            {
                if (_procedureStart != null)
                {
                    _procedureStart.RemoveUIForm(UIFormId.LoginLoadForm);
                }
            }
        }

        private void FinishLoading()
        {
            // 播放完成战鼓声
            GameEntry.Sound.PlaySound(finishSoundId);

            if (_procedureStart != null)
            {
                _procedureStart.StartGame();
            }
        }

        private void HandleTimeout()
        {
            _isFinished = true;

            // 先移除/关闭加载界面，避免其在弹窗打开被 Pause 时引发协程激活错误
            if (_procedureStart != null)
            {
                _procedureStart.RemoveUIForm(UIFormId.LoginLoadForm);
            }

            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Server.Error"),
                Message = GameEntry.Localization.GetString("Login.ConnectServerTimeout"),
                OnClickConfirm = _ =>
                {
                    var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
                    if (channel != null && channel.Connected)
                    {
                        channel.Close();
                    }
                }
            });
        }

        // --- Tips 轮播相关 ---
        private void ShowRandomTip(System.Action onComplete = null)
        {
            var tipProcessor = GameEntry.GameConfig.GetProcessor<LoadingTipProcessor>();
            if (tipProcessor == null)
            {
                onComplete?.Invoke();
                return;
            }

            var randomTip = tipProcessor.GetRandomTip();
            if (randomTip == null || string.IsNullOrEmpty(randomTip.DescFileId))
            {
                onComplete?.Invoke();
                return;
            }

            var assetPath = $"Assets/Game/Localization/{GameEntry.Localization.Language.ToString()}/Loading/{randomTip.DescFileId}.txt";
            GameEntry.Resource.LoadAsset(assetPath, typeof(TextAsset), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    var textAsset = asset as TextAsset;
                    if (textAsset != null && tipsText != null)
                    {
                        tipsText.text = textAsset.text;
                    }
                    onComplete?.Invoke();
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Warning("Load loading tip file failed: {0}, error: {1}", assetName, errorMessage);
                    onComplete?.Invoke();
                }
            ));
        }

        private IEnumerator CoRotateTips()
        {
            while (true)
            {
                // 使用 WaitForSecondsRealtime，防止由于加载状态下 timeScale = 0 导致协程暂停
                yield return new WaitForSecondsRealtime(3.0f);

                if (tipsCanvasGroup != null)
                {
                    // 0.3s 渐隐 (使用 unscaledDeltaTime 避免受 timeScale=0 影响)
                    var duration = 0.3f;
                    var elapsed = 0f;
                    while (elapsed < duration)
                    {
                        elapsed += Time.unscaledDeltaTime;
                        tipsCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                        yield return null;
                    }

                    tipsCanvasGroup.alpha = 0f;
                }

                // 异步替换为下一个权重随机提示语，加载完成后触发渐显
                var isLoaded = false;
                ShowRandomTip(() => { isLoaded = true; });

                // 等待直到异步加载完成
                while (!isLoaded)
                {
                    yield return null;
                }

                if (tipsCanvasGroup != null)
                {
                    // 0.3s 渐显 (使用 unscaledDeltaTime)
                    var duration = 0.3f;
                    var elapsed = 0f;
                    while (elapsed < duration)
                    {
                        elapsed += Time.unscaledDeltaTime;
                        tipsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                        yield return null;
                    }

                    tipsCanvasGroup.alpha = 1f;
                }
            }
        }
    }
}