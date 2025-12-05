using Game.Scripts.Main.Runtime.DataTable;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIItem.UICreate
{
    public class AvatarItem : ItemBase, IPointerClickHandler
    {
        [SerializeField]
        private Image imageBackground;

        [SerializeField]
        private Image imageAvatar;

        private object avatarHandle;
        private System.Action<int> onClick;
        private int selfIndex;

        public void SetData(int index, DRAvatar data, System.Action<int> clickCallback)
        {
            selfIndex = index;
            onClick = clickCallback;

            if (avatarHandle != null)
            {
                GameEntry.Resource.UnloadAsset(avatarHandle);
                avatarHandle = null;
            }

            GameEntry.Resource.LoadAsset(data.Path, typeof(Sprite), 0,
                new LoadAssetCallbacks(
                     (assetName, asset, duration, userData) =>
                    {
                        avatarHandle = asset;
                        imageAvatar.sprite = asset as Sprite;
                    },
                    (assetName, status, errorMessage, userData) =>
                    {
                        Log.Error($"头像加载失败:{errorMessage}");
                    }));
        }

        public void SetSelected(bool selected)
        {
            imageBackground.color = selected ? Color.yellow : Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(selfIndex);
        }

        public override void OnRecycle()
        {
            if (avatarHandle != null)
            {
                GameEntry.Resource.UnloadAsset(avatarHandle);
                avatarHandle = null;
            }
            imageAvatar.sprite = null;
            onClick = null;
        }
    }
}