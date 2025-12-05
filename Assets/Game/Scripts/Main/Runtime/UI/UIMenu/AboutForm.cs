using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class AboutForm : UGuiForm
    {
        [SerializeField]
        private RectTransform rectTransform = null;

        [SerializeField]
        private float scrollSpeed = 1f;

        private float initPosition = 0f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            var canvasScaler = GetComponentInParent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Log.Warning("Can not find CanvasScaler component.");
                return;
            }

            initPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            rectTransform.SetLocalPositionY(initPosition);

            // 换个音乐
            Base.GameEntry.Sound.PlayMusic(3);
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            // 还原音乐
            if (!isShutdown)
            {
                Base.GameEntry.Sound.PlayMusic(1);
            }
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            rectTransform.AddLocalPositionY(scrollSpeed * elapseSeconds);
            if (rectTransform.localPosition.y > rectTransform.sizeDelta.y - initPosition)
            {
                rectTransform.SetLocalPositionY(initPosition);
            }
        }
    }
}
