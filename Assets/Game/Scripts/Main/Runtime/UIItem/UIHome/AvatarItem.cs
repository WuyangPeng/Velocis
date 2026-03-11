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
    public class AvatarItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;

        [SerializeField] private Image imageAvatar;

        private object _avatarHandle;
        private Action<int> _onClick;
        private int _selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_selfIndex);
        }

        public void SetFrameSprite(string path)
        {
            GameEntry.Resource.LoadAsset(path, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                    (_, asset, _, _) => { imageBackground.sprite = asset as Sprite; },
                    (_, _, errorMessage, _) => { Log.Error($"头像加载失败:{errorMessage}"); }));
        }

        public void SetSprite(string path)
        {
            GameEntry.Resource.LoadAsset(path, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                    (_, asset, _, _) =>
                    {
                        _avatarHandle = asset;
                        imageAvatar.sprite = asset as Sprite;
                    },
                    (_, _, errorMessage, _) => { Log.Error($"头像加载失败:{errorMessage}"); }));
        }

        public void SetData(int index, avatar_config data, Action<int> clickCallback)
        {
            _selfIndex = index;
            _onClick = clickCallback;

            if (_avatarHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_avatarHandle);
                _avatarHandle = null;
            }

            GameEntry.Resource.LoadAsset(data.IconRes, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                    (_, asset, _, _) =>
                    {
                        _avatarHandle = asset;
                        imageAvatar.sprite = asset as Sprite;
                    },
                    (_, _, errorMessage, _) => { Log.Error($"头像加载失败:{errorMessage}"); }));
        }

        public void SetSelected(bool selected)
        {
            imageBackground.color = selected ? Color.yellow : Color.white;
        }

        public void SetGrayscale(bool isGrayscale)
        {
            imageAvatar.color = isGrayscale ? Color.gray : Color.white;
        }


        public override void OnRecycle()
        {
            if (_avatarHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_avatarHandle);
                _avatarHandle = null;
            }

            imageAvatar.sprite = null;
            imageAvatar.color = Color.white;
            _onClick = null;
        }
    }
}