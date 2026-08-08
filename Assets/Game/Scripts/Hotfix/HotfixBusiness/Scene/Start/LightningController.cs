// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using System.Collections;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.Scene.Start
{
    /// <summary>
    /// 闪电视觉与音效控制器。
    /// </summary>
    public class LightningController : MonoBehaviour
    {
        /// <summary>
        /// 闪电 UI 图片组件。
        /// </summary>
        [SerializeField] private Image lightningImage;

        /// <summary>
        /// 闪电随机出现的最小间隔时间（秒）。
        /// </summary>
        [SerializeField] private float minInterval = 1.5f;

        /// <summary>
        /// 闪电随机出现的最大间隔时间（秒）。
        /// </summary>
        [SerializeField] private float maxInterval = 4f;

        /// <summary>
        /// 闪电雷鸣音效 ID。
        /// </summary>
        [SerializeField] [SoundId] private int sound;

        /// <summary>
        /// 画布半宽度，用于控制闪电在屏幕内的水平生成范围。
        /// </summary>
        private float _canvasHalfWidth;

        /// <summary>
        /// 当前进行闪烁表现的协程引用。
        /// </summary>
        private Coroutine _flashRoutine;

        /// <summary>
        /// 距离下一次闪电触发的倒计时计时器。
        /// </summary>
        private float _timer;

        /// <summary>
        /// 游戏物体初始化逻辑。
        /// </summary>
        private void Start()
        {
            if (lightningImage != null)
            {
                // 默认隐藏
                lightningImage.gameObject.SetActive(false);

                // 动态获取画布半宽，以便闪电可以在整个屏幕宽度内生成
                var canvasRect = lightningImage.canvas?.GetComponent<RectTransform>();
                if (canvasRect)
                {
                    _canvasHalfWidth = canvasRect.rect.width * 0.5f;
                }
                else
                {
                    Log.Warning("LightningController: lightningImage is not under a Canvas!");
                }
            }
            else
            {
                Debug.LogWarning("LightningController: lightningImage not assigned!");
            }

            _timer = Random.Range(minInterval, maxInterval);
        }

        /// <summary>
        /// 轮询更新逻辑，处理闪电的定时触发。
        /// </summary>
        private void Update()
        {
            _timer -= Time.deltaTime;
            if (!(_timer <= 0f))
            {
                return;
            }

            if (_flashRoutine != null)
            {
                StopCoroutine(_flashRoutine);
            }

            _flashRoutine = StartCoroutine(DoFlash());
            _timer = Random.Range(minInterval, maxInterval);
        }

        /// <summary>
        /// 触发一次完整的闪电双闪表现（包含位置随机、闪烁和音效播放）。
        /// </summary>
        private IEnumerator DoFlash()
        {
            if (!lightningImage || _canvasHalfWidth <= 0f)
            {
                yield break;
            }

            RandomizeTransform(lightningImage.rectTransform);

            // 第一次闪烁（快速亮起）
            yield return FlashOnce(0.08f, playSound: true);

            // 极短间隔（第二次闪击前的黑暗）
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            // 第二次闪烁（稍微长一点的闪烁）
            yield return FlashOnce(0.15f);
        }

        /// <summary>
        /// 随机化闪电的位置、缩放、翻转及旋转角度。
        /// </summary>
        /// <param name="rect">要随机化变换的 RectTransform 目标。</param>
        private void RandomizeTransform(RectTransform rect)
        {
            // 在整个屏幕宽度内随机化位置
            // 保持在天空（上半屏）
            rect.anchoredPosition = new Vector2(
                Random.Range(-_canvasHalfWidth + 100f, _canvasHalfWidth - 100f),
                Random.Range(100f, 300f)
            );

            // 随机化缩放（大小以及水平翻转）
            var scaleX = Random.Range(0.8f, 1.6f) * (Random.value > 0.5f ? 1f : -1f);
            var scaleY = Random.Range(0.9f, 1.5f);
            rect.localScale = new Vector3(scaleX, scaleY, 1f);

            // 随机化旋转角度（Z轴旋转在 -20 到 20 度之间）
            rect.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
        }

        /// <summary>
        /// 控制闪电单次亮起并维持指定的时间。
        /// </summary>
        /// <param name="duration">亮起持续时间，单位秒。</param>
        /// <param name="playSound">是否在亮起时播放音效。</param>
        private IEnumerator FlashOnce(float duration, bool playSound = false)
        {
            lightningImage.gameObject.SetActive(true);
            if (playSound)
            {
                GameEntry.Sound.PlaySound(sound);
            }

            yield return new WaitForSeconds(duration);
            lightningImage.gameObject.SetActive(false);
        }
    }
}