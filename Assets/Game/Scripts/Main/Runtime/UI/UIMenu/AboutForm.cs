using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class AboutForm : UGuiForm
    {
        [SerializeField] private RectTransform rectTransform;

        [SerializeField] private float scrollSpeed = 1f;

        private float _initPosition;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            var canvasScaler = GetComponentInParent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Log.Warning("Can not find CanvasScaler component.");
                return;
            }

            _initPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            rectTransform.SetLocalPositionY(_initPosition);

            // 换个音乐
            GameEntry.Sound.PlayMusic(3);
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            // 还原音乐
            if (!isShutdown)
            {
                GameEntry.Sound.PlayMusic(1);
            }
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            rectTransform.AddLocalPositionY(scrollSpeed * elapseSeconds);
            if (rectTransform.localPosition.y > rectTransform.sizeDelta.y - _initPosition)
            {
                rectTransform.SetLocalPositionY(_initPosition);
            }
        }
    }
}