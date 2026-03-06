using System;
using Celeritas.Config.game;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIItem.UICreate
{
    public class FrameItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageFrame;

        private object frameHandle;
        private Action<int> onClick;
        private int selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(selfIndex);
        }

        public void SetData(int index, frame_config data, Action<int> clickCallback)
        {
            selfIndex = index;
            onClick = clickCallback;

            if (frameHandle != null)
            {
                GameEntry.Resource.UnloadAsset(frameHandle);
                frameHandle = null;
            }

            GameEntry.Resource.LoadAsset(data.IconRes, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                    (assetName, asset, duration, userData) =>
                    {
                        frameHandle = asset;
                        imageFrame.sprite = asset as Sprite;
                    },
                    (assetName, status, errorMessage, userData) => { Log.Error($"头像框加载失败:{errorMessage}"); }));
        }

        public void SetSelected(bool selected)
        {
            imageBackground.color = selected ? Color.yellow : Color.white;
        }

        public void SetGrayscale(bool isGrayscale)
        {
            imageFrame.color = isGrayscale ? Color.gray : Color.white;
        }

        public override void OnRecycle()
        {
            if (frameHandle != null)
            {
                GameEntry.Resource.UnloadAsset(frameHandle);
                frameHandle = null;
            }

            imageFrame.sprite = null;
            imageFrame.color = Color.white;
            onClick = null;
        }
    }
}
