using System.Collections;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.HPBar
{
    public class HpBarItem : MonoBehaviour
    {
        private const float AnimationSeconds = 0.3f;
        private const float KeepSeconds = 0.4f;
        private const float FadeOutSeconds = 0.3f;

        [SerializeField]
        private Slider hpBar;

        private Canvas mParentCanvas;
        private RectTransform cachedTransform;
        private CanvasGroup cachedCanvasGroup;
        private int ownerId;

        public Entity.EntityLogic.Entity Owner { get; private set; }

        public void Init(Entity.EntityLogic.Entity owner, Canvas parentCanvas, float fromHpRatio, float toHpRatio)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            mParentCanvas = parentCanvas;

            gameObject.SetActive(true);
            StopAllCoroutines();

            cachedCanvasGroup.alpha = 1f;
            if (Owner != owner || ownerId != owner.Id)
            {
                hpBar.value = fromHpRatio;
                Owner = owner;
                ownerId = owner.Id;
            }

            Refresh();

            StartCoroutine(HpBarCo(toHpRatio, AnimationSeconds, KeepSeconds, FadeOutSeconds));
        }

        public bool Refresh()
        {
            if (cachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (Owner == null || !Owner.Available || Owner.Id != ownerId) return true;
            var worldPosition = Owner.CachedTransform.position + Vector3.forward;
            var screenPosition = Base.GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)mParentCanvas.transform, screenPosition, mParentCanvas.worldCamera, out var position))
            {
                cachedTransform.localPosition = position;
            }

            return true;
        }

        public void Reset()
        {
            StopAllCoroutines();
            cachedCanvasGroup.alpha = 1f;
            hpBar.value = 1f;
            Owner = null;
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            cachedTransform = GetComponent<RectTransform>();
            if (cachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            cachedCanvasGroup = GetComponent<CanvasGroup>();
            if (cachedCanvasGroup != null)
            {
                return;
            }
            Log.Error("CanvasGroup is invalid.");
        }

        private IEnumerator HpBarCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
        {
            yield return hpBar.SmoothValue(value, animationDuration);
            yield return new WaitForSeconds(keepDuration);
            yield return cachedCanvasGroup.FadeToAlpha(0f, fadeOutDuration);
        }
    }
}
