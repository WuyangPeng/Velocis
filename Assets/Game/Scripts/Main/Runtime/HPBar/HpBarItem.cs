using System.Collections;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.HPBar
{
    public class HpBarItem : MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 0.4f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField] private Slider hpBar;

        private CanvasGroup _cachedCanvasGroup;
        private RectTransform _cachedTransform;
        private int _ownerId;

        private Canvas _parentCanvas;

        public Entity.EntityLogic.Entity Owner { get; private set; }

        private void Awake()
        {
            _cachedTransform = GetComponent<RectTransform>();
            if (_cachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            _cachedCanvasGroup = GetComponent<CanvasGroup>();
            if (_cachedCanvasGroup != null)
            {
                return;
            }

            Log.Error("CanvasGroup is invalid.");
        }

        public void Reset()
        {
            StopAllCoroutines();
            _cachedCanvasGroup.alpha = 1f;
            hpBar.value = 1f;
            Owner = null;
            gameObject.SetActive(false);
        }

        public void Init(Entity.EntityLogic.Entity owner, Canvas parentCanvas, float fromHpRatio, float toHpRatio)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            _parentCanvas = parentCanvas;

            gameObject.SetActive(true);
            StopAllCoroutines();

            _cachedCanvasGroup.alpha = 1f;
            if (Owner != owner || _ownerId != owner.Id)
            {
                hpBar.value = fromHpRatio;
                Owner = owner;
                _ownerId = owner.Id;
            }

            Refresh();

            StartCoroutine(HpBarCo(toHpRatio, AnimationSeconds, KeepSeconds, FadeOutSeconds));
        }

        public bool Refresh()
        {
            if (_cachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (Owner == null || !Owner.Available || Owner.Id != _ownerId)
            {
                return true;
            }

            var worldPosition = Owner.CachedTransform.position + Vector3.forward;
            var screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_parentCanvas.transform, screenPosition, _parentCanvas.worldCamera, out var position))
            {
                _cachedTransform.localPosition = position;
            }

            return true;
        }

        private IEnumerator HpBarCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
        {
            yield return hpBar.SmoothValue(value, animationDuration);
            yield return new WaitForSeconds(keepDuration);
            yield return _cachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }
    }
}