using System;
using Celeritas.Config.game;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIItem.UIHome
{
    public class FrameItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imageFrame;

        private object _frameHandle;
        private Action<int> _onClick;
        private int _selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_selfIndex);
        }

        public void SetData(int index, frame_config data, Action<int> clickCallback)
        {
            _selfIndex = index;
            _onClick = clickCallback;

            if (_frameHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_frameHandle);
                _frameHandle = null;
            }

            GameEntry.Resource.LoadAsset(data.IconRes, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                    (_, asset, _, _) =>
                    {
                        _frameHandle = asset;
                        imageFrame.sprite = asset as Sprite;
                    },
                    (_, _, errorMessage, _) => { Log.Error($"头像框加载失败:{errorMessage}"); }));
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
            if (_frameHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_frameHandle);
                _frameHandle = null;
            }

            imageFrame.sprite = null;
            imageFrame.color = Color.white;
            _onClick = null;
        }
    }
}