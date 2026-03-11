using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UICommon
{
    /// <summary>
    ///     uGUI 界面组辅助器。
    /// </summary>
    public class UGuiGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;
        private Canvas _cachedCanvas;

        private int _depth;

        private void Awake()
        {
            _cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = DepthFactor * _depth;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }

        /// <summary>
        ///     设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            _depth = depth;
            _cachedCanvas.overrideSorting = true;
            _cachedCanvas.sortingOrder = DepthFactor * depth;
        }
    }
}