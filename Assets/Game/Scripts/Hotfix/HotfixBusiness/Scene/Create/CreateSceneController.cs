// 创建时间：2026-08-07
// 修改时间：2026-08-07
// 审核时间：

using System.Collections;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.Scene.Create
{
    /// <summary>
    /// 狂风大雨场景视觉与音效控制器。
    /// </summary>
    public class CreateSceneController : MonoBehaviour
    {
        /// <summary>
        /// 下大雨粒子系统。
        /// </summary>
        [SerializeField] private ParticleSystem heavyRainParticles;

        /// <summary>
        /// 狂风吹拂粒子系统。
        /// </summary>
        [SerializeField] private ParticleSystem galeWindParticles;

        /// <summary>
        /// 雨滴溅起粒子系统。
        /// </summary>
        [SerializeField] private ParticleSystem rainSplashParticles;

        /// <summary>
        /// 狂风风声音效 ID。
        /// </summary>
        [SerializeField] [SoundId] private int windSound;

        /// <summary>
        /// 暴雨环境音效 ID。
        /// </summary>
        [SerializeField] [SoundId] private int rainSound;

        /// <summary>
        /// 阵风变化计时器。
        /// </summary>
        private float _gustTimer;

        /// <summary>
        /// 基础雨滴发射速率。
        /// </summary>
        private float _baseRainRate = 350f;

        /// <summary>
        /// 基础狂风发射速率。
        /// </summary>
        private float _baseWindRate = 25f;

        private void Start()
        {
            if (windSound > 0)
            {
                GameEntry.Sound.PlaySound(windSound);
            }

            if (rainSound > 0)
            {
                GameEntry.Sound.PlaySound(rainSound);
            }

            if (heavyRainParticles != null)
            {
                var emission = heavyRainParticles.emission;
                _baseRainRate = emission.rateOverTime.constant;
            }

            if (galeWindParticles != null)
            {
                var emission = galeWindParticles.emission;
                _baseWindRate = emission.rateOverTime.constant;
            }

            _gustTimer = Random.Range(2f, 5f);
        }

        private void Update()
        {
            // 模拟强雷暴天气中强阵风的阵阵袭来与起伏
            _gustTimer -= Time.deltaTime;
            if (_gustTimer <= 0f)
            {
                _gustTimer = Random.Range(3f, 7f);
                StartCoroutine(SimulateWindGust());
            }
        }

        /// <summary>
        /// 模拟一次强阵风爆发与衰减过程。
        /// </summary>
        private IEnumerator SimulateWindGust()
        {
            float gustMultiplier = Random.Range(1.3f, 1.8f);
            float duration = Random.Range(1.5f, 3.5f);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float factor = Mathf.Sin((elapsed / duration) * Mathf.PI);
                float currentMultiplier = Mathf.Lerp(1f, gustMultiplier, factor);

                if (heavyRainParticles != null)
                {
                    var emission = heavyRainParticles.emission;
                    emission.rateOverTime = _baseRainRate * currentMultiplier;
                }

                if (galeWindParticles != null)
                {
                    var emission = galeWindParticles.emission;
                    emission.rateOverTime = _baseWindRate * currentMultiplier;
                }

                yield return null;
            }

            // 恢复基础速率
            if (heavyRainParticles != null)
            {
                var emission = heavyRainParticles.emission;
                emission.rateOverTime = _baseRainRate;
            }

            if (galeWindParticles != null)
            {
                var emission = galeWindParticles.emission;
                emission.rateOverTime = _baseWindRate;
            }
        }
    }
}
