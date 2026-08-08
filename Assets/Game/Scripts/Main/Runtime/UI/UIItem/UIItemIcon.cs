using System;
// using Celeritas.Config;
// using Celeritas.Config.game;
using Game.Scripts.Main.Runtime.UIItem;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIItem
{
    /// <summary>
    ///     道具通用 Icon 组件。
    /// </summary>
    public class UIItemIcon : ItemBase, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        private object _borderAssetHandle;

        private object _iconAssetHandle;
        private bool _isDisabled;

        private Action<UIItemIcon> _onClick;
        private Action<UIItemIcon> _onHoverEnter;
        private Action<UIItemIcon> _onHoverExit;
        [SerializeField] private Image imageBorder;
        [SerializeField] private Image imageIcon;
        [SerializeField] private GameObject objEquipped;
        [SerializeField] private GameObject objHover;
        [SerializeField] private GameObject objLimitTime;
        [SerializeField] private GameObject objLocked;
        [SerializeField] private GameObject objNew;
        [SerializeField] private Text textCount;

        // private item_config Config { get; set; }

        private int Count { get; set; }

        private bool IsLocked { get; set; }

        private bool IsEquipped { get; set; }

        private bool IsNew { get; set; }

        private bool IsLimitTime { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isDisabled)
            {
                return;
            }

            _onClick?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isDisabled)
            {
                return;
            }

            transform.localScale = new Vector3(0.95f, 0.95f, 1f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isDisabled)
            {
                return;
            }

            if (objHover != null)
            {
                objHover.SetActive(true);
            }

            _onHoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDisabled)
            {
                return;
            }

            if (objHover != null)
            {
                objHover.SetActive(false);
            }

            _onHoverExit?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDisabled)
            {
                return;
            }

            transform.localScale = Vector3.one;
        }

        public void SetData(object config, int count, bool isLocked = false, bool isEquipped = false, bool isNew = false, bool isLimitTime = false, bool isDisabled = false, Action<UIItemIcon> onClick = null, Action<UIItemIcon> onHoverEnter = null, Action<UIItemIcon> onHoverExit = null)
        {
            // Config = config;
            Count = count;
            IsLocked = isLocked;
            IsEquipped = isEquipped;
            IsNew = isNew;
            IsLimitTime = isLimitTime;
            _isDisabled = isDisabled;
            _onClick = onClick;
            _onHoverEnter = onHoverEnter;
            _onHoverExit = onHoverExit;

            RefreshUI();
        }

        private void RefreshUI()
        {
            // if (Config == null)
            // {
            //     gameObject.SetActive(false);
            //     return;
            // }

            gameObject.SetActive(true);
        }

        // private string GetBorderAssetPath(quality_type quality)
        // {
        // }

        private void UnloadIcon()
        {
            if (_iconAssetHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_iconAssetHandle);
                _iconAssetHandle = null;
            }

            if (imageIcon != null)
            {
                imageIcon.sprite = null;
            }
        }

        private void UnloadBorder()
        {
            if (_borderAssetHandle != null)
            {
                GameEntry.Resource.UnloadAsset(_borderAssetHandle);
                _borderAssetHandle = null;
            }

            if (imageBorder != null)
            {
                imageBorder.sprite = null;
            }
        }

        public override void OnRecycle()
        {
            UnloadIcon();
            UnloadBorder();

        //    Config = null;
            Count = 0;
            IsLocked = false;
            IsEquipped = false;
            IsNew = false;
            IsLimitTime = false;
            _isDisabled = false;

            _onClick = null;
            _onHoverEnter = null;
            _onHoverExit = null;

            if (objHover != null)
            {
                objHover.SetActive(false);
            }
        }
    }
}