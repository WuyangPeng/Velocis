// 创建时间：2026-08-07
// 修改时间：2026-08-07

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Scripts.Hotfix.HotfixBusiness.Tools.Button
{
    /// <summary>
    /// 按钮 Hover 悬停粒子效果工具类
    /// 用于动态生成、管理与更新按钮悬停时的光斑/闪烁粒子动画
    /// </summary>
    public static class ButtonHoverParticleUtility
    {
        /// <summary> 悬停粒子容器节点名称 </summary>
        private const string HoverParticlesNodeName = "HoverParticles";
        
        /// <summary> 容器宽度相对按钮宽度的扩展比例 </summary>
        private const float ExpandWidthRatio = 0.44f;
        
        /// <summary> 容器高度相对按钮高度的扩展比例 </summary>
        private const float ExpandHeightRatio = 0.82f;
        
        /// <summary> 粒子最小尺寸 </summary>
        private const float MinParticleSize = 14f;
        
        /// <summary> 粒子最大尺寸 </summary>
        private const float MaxParticleSize = 30f;
        
        /// <summary> 闪烁最小缩放系数 </summary>
        private const float MinSparkleScale = 0.5f;
        
        /// <summary> 闪烁最大缩放系数 </summary>
        private const float MaxSparkleScale = 1.85f;
        
        /// <summary> 默认扩展容器尺寸 </summary>
        private static readonly Vector2 DefaultExpandSize = new(110f, 58f);

        /// <summary>
        /// 确保按钮根节点下拥有指定数量的悬停粒子（按需实例化模板或销毁多余节点）
        /// </summary>
        /// <param name="root">按钮根节点 Transform</param>
        /// <param name="sparkleTemplate">粒子模板 GameObject</param>
        /// <param name="count">需要的粒子总数量</param>
        /// <returns>容器下所有粒子的 Image 组件数组</returns>
        public static Image[] EnsureHoverParticles(Transform root, GameObject sparkleTemplate, int count)
        {
            if (count <= 0 || sparkleTemplate == null)
            {
                return Array.Empty<Image>();
            }

            var container = EnsureContainer(root);
            SyncContainerExpand(root, container);
            ConfigureTextSortingForParticles(root, container);
            var particles = container.GetComponentsInChildren<Image>(true);

            for (var i = particles.Length - 1; i >= count; i--)
            {
                Object.Destroy(particles[i].gameObject);
            }

            particles = container.GetComponentsInChildren<Image>(true);
            for (var i = particles.Length; i < count; i++)
            {
                CreateSparkleFromTemplate(container, sparkleTemplate, i);
            }

            container.gameObject.SetActive(false);
            return container.GetComponentsInChildren<Image>(true);
        }

        /// <summary>
        /// 播放按钮悬停粒子动画
        /// </summary>
        /// <param name="host">运行协程的宿主 MonoBehaviour</param>
        /// <param name="coroutine">动画协程引用（ref 传入以确保可复用或停止前一次运行）</param>
        /// <param name="particles">粒子 Image 组件数组</param>
        /// <param name="intensity">粒子透明度与效果强度系数</param>
        public static void PlayHover(MonoBehaviour host, ref Coroutine coroutine, Image[] particles, float intensity)
        {
            Stop(host, ref coroutine);
            if (particles == null || particles.Length == 0)
            {
                return;
            }

            var container = particles[0].transform.parent;
            if (container != null)
            {
                container.gameObject.SetActive(true);
            }

            foreach (var particle in particles)
            {
                if (particle == null)
                {
                    continue;
                }

                particle.gameObject.SetActive(true);
            }

            coroutine = host.StartCoroutine(AnimateParticles(particles, intensity));
        }

        /// <summary>
        /// 停止按钮悬停粒子动画并隐藏粒子
        /// </summary>
        /// <param name="host">运行协程的宿主 MonoBehaviour</param>
        /// <param name="coroutine">动画协程引用</param>
        /// <param name="particles">粒子 Image 组件数组</param>
        public static void StopHover(MonoBehaviour host, ref Coroutine coroutine, Image[] particles)
        {
            Stop(host, ref coroutine);
            if (particles == null)
            {
                return;
            }

            foreach (var particle in particles)
            {
                if (particle == null)
                {
                    continue;
                }

                var color = particle.color;
                color.a = 0f;
                particle.color = color;
                particle.gameObject.SetActive(false);
            }

            if (particles.Length <= 0)
            {
                return;
            }

            var container = particles[0].transform.parent;
            if (container != null)
            {
                container.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 重置悬停粒子状态（关闭悬停效果）
        /// </summary>
        /// <param name="host">运行协程的宿主 MonoBehaviour</param>
        /// <param name="coroutine">动画协程引用</param>
        /// <param name="particles">粒子 Image 组件数组</param>
        public static void Reset(MonoBehaviour host, ref Coroutine coroutine, Image[] particles)
        {
            StopHover(host, ref coroutine, particles);
        }

        /// <summary>
        /// 停止指定协程并置空引用
        /// </summary>
        /// <param name="host">协程宿主 MonoBehaviour</param>
        /// <param name="coroutine">要停止的协程引用</param>
        private static void Stop(MonoBehaviour host, ref Coroutine coroutine)
        {
            if (coroutine == null)
            {
                return;
            }

            host.StopCoroutine(coroutine);
            coroutine = null;
        }

        /// <summary>
        /// 粒子循环动画协程
        /// </summary>
        /// <param name="particles">粒子 Image 组件数组</param>
        /// <param name="intensity">强度系数</param>
        /// <returns>IEnumerator 协程对象</returns>
        private static IEnumerator AnimateParticles(Image[] particles, float intensity)
        {
            var states = new ParticleState[particles.Length];

            for (var i = 0; i < particles.Length; i++)
            {
                states[i].image = particles[i];
                states[i].rect = particles[i] ? particles[i].rectTransform : null;
                ResetParticle(ref states[i], true);
            }

            while (true)
            {
                var anyActive = false;
                for (var i = 0; i < states.Length; i++)
                {
                    if (!states[i].image)
                    {
                        continue;
                    }

                    anyActive = true;

                    UpdateParticleState(ref states[i], intensity);
                }

                if (!anyActive)
                {
                    yield break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// 更新单个粒子的运动状态（位置、自转、缩放及透明度）
        /// </summary>
        /// <param name="state">粒子状态结构体引用</param>
        /// <param name="intensity">透明度/强度系数</param>
        private static void UpdateParticleState(ref ParticleState state, float intensity)
        {
            state.elapsedTime += Time.unscaledDeltaTime;
            if (state.elapsedTime >= state.duration)
            {
                ResetParticle(ref state, false);
            }

            var progress = Mathf.Clamp01(state.elapsedTime / state.duration);
            var sparkle = Mathf.Sin(progress * Mathf.PI);
            state.rect.anchoredPosition = Vector2.Lerp(state.startPosition, state.endPosition, progress);
            state.rect.localRotation = Quaternion.Euler(0f, 0f, state.rect.localEulerAngles.z + state.spinSpeed * Time.unscaledDeltaTime);
            state.rect.localScale = Vector3.one * Mathf.Lerp(MinSparkleScale, MaxSparkleScale, sparkle);

            var color = state.image.color;
            color.a = sparkle * intensity;
            state.image.color = color;
        }

        /// <summary>
        /// 随机重置粒子的生命周期、轨迹起始/终点位置、大小与旋转速度
        /// </summary>
        /// <param name="state">粒子状态结构体引用</param>
        /// <param name="randomizeProgress">是否随机生成初始已播时间（用于首次播时错开各个粒子的相位）</param>
        private static void ResetParticle(ref ParticleState state, bool randomizeProgress)
        {
            if (!state.rect)
            {
                return;
            }

            GetParticleBounds(state.rect, out var halfWidth, out var halfHeight, out var sizeScale);
            var innerWidth = halfWidth * 0.75f;
            var innerHeight = halfHeight * 0.75f;
            var driftScale = Mathf.Max(sizeScale, 1f);

            Vector2 start;
            Vector2 end;
            if (Random.value < 0.45f)
            {
                start = new Vector2(
                    Random.Range(-innerWidth, innerWidth),
                    Random.Range(-innerHeight, innerHeight));
                end = start + new Vector2(Random.Range(-10f, 10f) * driftScale, Random.Range(-8f, 8f) * driftScale);
            }
            else
            {
                var side = Random.Range(0, 4);
                var edgeJitter = Random.Range(-1f, 1f);
                start = side switch
                {
                    0 => new Vector2(edgeJitter * halfWidth, halfHeight),
                    1 => new Vector2(edgeJitter * halfWidth, -halfHeight),
                    2 => new Vector2(-halfWidth, edgeJitter * halfHeight),
                    _ => new Vector2(halfWidth, edgeJitter * halfHeight)
                };

                var yDrift = side switch
                {
                    0 => Random.Range(-14f, 4f) * driftScale,
                    1 => Random.Range(-4f, 8f) * driftScale,
                    _ => Random.Range(-10f, 8f) * driftScale
                };
                end = start + new Vector2(Random.Range(-14f, 14f) * driftScale, yDrift);
            }

            state.startPosition = start;
            state.endPosition = end;
            state.duration = Random.Range(0.42f, 0.82f);
            state.elapsedTime = randomizeProgress ? Random.Range(0f, state.duration) : 0f;
            state.size = Random.Range(MinParticleSize, MaxParticleSize) * sizeScale;
            state.spinSpeed = Random.Range(-260f, 260f);

            state.rect.anchoredPosition = state.startPosition;
            state.rect.sizeDelta = Vector2.one * state.size;
            state.rect.localScale = Vector3.zero;
        }

        /// <summary>
        /// 获取粒子容器的边界信息以及粒子尺寸缩放比
        /// </summary>
        /// <param name="particleRect">粒子的 RectTransform</param>
        /// <param name="halfWidth">输出半宽</param>
        /// <param name="halfHeight">输出半高</param>
        /// <param name="sizeScale">输出粒子整体尺寸缩放系数</param>
        private static void GetParticleBounds(RectTransform particleRect, out float halfWidth, out float halfHeight, out float sizeScale)
        {
            halfWidth = 102f;
            halfHeight = 42f;
            sizeScale = 1f;

            if (particleRect.parent is not RectTransform container)
            {
                return;
            }

            var rect = container.rect;
            if (rect.width <= 0f || rect.height <= 0f)
            {
                return;
            }

            halfWidth = rect.width * 0.5f;
            halfHeight = rect.height * 0.5f;
            sizeScale = Mathf.Max(1.2f, Mathf.Min(halfWidth, halfHeight) / 35f);
        }

        /// <summary>
        /// 根据按钮根节点尺寸同步调整粒子容器尺寸
        /// </summary>
        /// <param name="root">按钮根节点 Transform</param>
        /// <param name="container">粒子容器 Transform</param>
        private static void SyncContainerExpand(Transform root, Transform container)
        {
            if (root is not RectTransform buttonRect || container is not RectTransform particlesRect)
            {
                return;
            }

            var rect = buttonRect.rect;
            if (rect.width <= 0f || rect.height <= 0f)
            {
                return;
            }

            particlesRect.sizeDelta = new Vector2(rect.width * ExpandWidthRatio, rect.height * ExpandHeightRatio);
        }

        /// <summary>
        /// 确保按钮根节点下创建或获取粒子容器 GameObject
        /// </summary>
        /// <param name="root">按钮根节点 Transform</param>
        /// <returns>粒子容器 Transform</returns>
        private static Transform EnsureContainer(Transform root)
        {
            var container = root.Find(HoverParticlesNodeName);
            if (container != null)
            {
                return container;
            }

            var particlesGo = new GameObject(HoverParticlesNodeName)
            {
                layer = root.gameObject.layer
            };
            particlesGo.transform.SetParent(root, false);

            var particlesRect = particlesGo.AddComponent<RectTransform>();
            particlesRect.anchorMin = Vector2.zero;
            particlesRect.anchorMax = Vector2.one;
            particlesRect.anchoredPosition = Vector2.zero;
            particlesRect.sizeDelta = DefaultExpandSize;
            particlesRect.pivot = new Vector2(0.5f, 0.5f);
            return particlesGo.transform;
        }

        /// <summary>
        /// 配置按钮文本及粒子容器 Canvas 层级排序，避免悬停粒子覆盖文本
        /// </summary>
        /// <param name="root">按钮根节点 Transform</param>
        /// <param name="container">粒子容器 Transform</param>
        private static void ConfigureTextSortingForParticles(Transform root, Transform container)
        {
            var text = root.Find("Text")?.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.geometrySortingOrder = VertexSortingOrder.Normal;
            }

            var canvas = container.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = container.gameObject.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = 10;
        }

        /// <summary>
        /// 根据模板实例化单个闪烁粒子 GameObject
        /// </summary>
        /// <param name="parent">粒子容器 Transform</param>
        /// <param name="sparkleTemplate">粒子模板 GameObject</param>
        /// <param name="index">粒子索引</param>
        private static void CreateSparkleFromTemplate(Transform parent, GameObject sparkleTemplate, int index)
        {
            var sparkleGo = Object.Instantiate(sparkleTemplate, parent, false);
            sparkleGo.name = $"GoldSparkle_{index:00}";
            sparkleGo.SetActive(false);
        }
    }
}