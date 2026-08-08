// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.Tools.Button
{
    /// <summary>
    /// 按钮悬停发光动效辅助工具类，提供缩放、透明度渐变等呼吸发光动效。
    /// </summary>
    public static class ButtonHoverGlowUtility
    {
        /// <summary>
        /// 停止指定的主体上正在运行的协程，并清空协程引用。
        /// </summary>
        /// <param name="host">运行协程的 MonoBehaviour 宿主</param>
        /// <param name="coroutine">被运行协程的引用，执行完毕会被置空</param>
        public static void Stop(MonoBehaviour host, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                return;
            }

            host.StopCoroutine(coroutine);
            coroutine = null;
        }

        /// <summary>
        /// 渐变改变发光图片的透明度到目标值。
        /// </summary>
        /// <param name="host">运行协程的 MonoBehaviour 宿主</param>
        /// <param name="coroutine">被运行协程的引用，执行完毕会被置空</param>
        /// <param name="image">目标发光图片</param>
        /// <param name="targetAlpha">目标透明度</param>
        /// <param name="duration">渐变持续时间</param>
        public static void FadeAlpha(MonoBehaviour host, ref Coroutine coroutine, Image image, float targetAlpha, float duration)
        {
            Stop(host, ref coroutine);
            if (image == null)
            {
                return;
            }

            coroutine = host.StartCoroutine(FadeAlphaRoutine(image, targetAlpha, duration));
        }

        /// <summary>
        /// 让发光图片进行周期性的缩放微动/呼吸效果。
        /// </summary>
        /// <param name="host">运行协程的 MonoBehaviour 宿主</param>
        /// <param name="coroutine">被运行协程的引用，执行完毕会被置空</param>
        /// <param name="rect">发光图片的 RectTransform</param>
        /// <param name="minScale">最小缩放值</param>
        /// <param name="maxScale">最大缩放值</param>
        /// <param name="period">单次脉冲变化周期时间</param>
        public static void PulseScale(MonoBehaviour host, ref Coroutine coroutine, RectTransform rect, float minScale, float maxScale, float period)
        {
            Stop(host, ref coroutine);
            if (rect == null)
            {
                return;
            }

            coroutine = host.StartCoroutine(PulseScaleRoutine(rect, minScale, maxScale, period));
        }

        /// <summary>
        /// 让发光图片透明度进行呼吸式的周期改变。
        /// </summary>
        /// <param name="host">运行协程的 MonoBehaviour 宿主</param>
        /// <param name="coroutine">被运行协程的引用，执行完毕会被置空</param>
        /// <param name="image">目标发光图片</param>
        /// <param name="minAlpha">最小透明度值</param>
        /// <param name="maxAlpha">最大透明度值</param>
        /// <param name="period">单次脉冲变化周期时间</param>
        public static void PulseAlpha(MonoBehaviour host, ref Coroutine coroutine, Image image, float minAlpha, float maxAlpha, float period)
        {
            Stop(host, ref coroutine);
            if (image == null)
            {
                return;
            }

            coroutine = host.StartCoroutine(PulseAlphaRoutine(image, minAlpha, maxAlpha, period));
        }

        /// <summary>
        /// 重置发光组件的状态（透明度设为 0，缩放设为 1）。
        /// </summary>
        /// <param name="image">目标发光图片</param>
        /// <param name="rect">发光图片的 RectTransform</param>
        public static void ResetGlow(Image image, RectTransform rect)
        {
            if (image != null)
            {
                var color = image.color;
                color.a = 0f;
                image.color = color;
            }

            if (rect != null)
            {
                rect.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// 透明度渐变的具体协程实现。
        /// </summary>
        private static IEnumerator FadeAlphaRoutine(Image image, float targetAlpha, float duration)
        {
            var startAlpha = image.color.a;
            var time = 0f;
            var color = image.color;
            while (time < duration)
            {
                time += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                image.color = color;
                yield return null;
            }

            color.a = targetAlpha;
            image.color = color;
        }

        /// <summary>
        /// 周期性缩放脉冲/呼吸效果的具体协程实现。
        /// </summary>
        private static IEnumerator PulseScaleRoutine(RectTransform rect, float minScale, float maxScale, float period)
        {
            var halfPeriod = period * 0.5f;
            while (true)
            {
                yield return LerpScale(rect, minScale, maxScale, halfPeriod);
                if (!rect)
                {
                    yield break;
                }

                yield return LerpScale(rect, maxScale, minScale, halfPeriod);
            }
        }

        /// <summary>
        /// 周期性透明度脉冲/呼吸效果的具体协程实现。
        /// </summary>
        private static IEnumerator PulseAlphaRoutine(Image image, float minAlpha, float maxAlpha, float period)
        {
            var halfPeriod = period * 0.5f;
            while (true)
            {
                yield return LerpAlpha(image, minAlpha, maxAlpha, halfPeriod);
                if (!image)
                {
                    yield break;
                }

                yield return LerpAlpha(image, maxAlpha, minAlpha, halfPeriod);
            }
        }

        /// <summary>
        /// 对指定的 RectTransform 执行缩放插值过渡。
        /// </summary>
        private static IEnumerator LerpScale(RectTransform rect, float from, float to, float duration)
        {
            var time = 0f;
            while (time < duration)
            {
                if (!rect)
                {
                    yield break;
                }

                time += Time.deltaTime;
                var scale = Mathf.Lerp(from, to, time / duration);
                rect.localScale = new Vector3(scale, scale, 1f);
                yield return null;
            }

            if (rect)
            {
                rect.localScale = new Vector3(to, to, 1f);
            }
        }

        /// <summary>
        /// 对指定的 Image 执行透明度插值过渡。
        /// </summary>
        private static IEnumerator LerpAlpha(Image image, float from, float to, float duration)
        {
            var time = 0f;
            Color color;
            while (time < duration)
            {
                if (!image)
                {
                    yield break;
                }

                time += Time.deltaTime;
                color = image.color;
                color.a = Mathf.Lerp(from, to, time / duration);
                image.color = color;
                yield return null;
            }

            if (!image)
            {
                yield break;
            }

            color = image.color;
            color.a = to;
            image.color = color;
        }
    }
}