using System.Collections;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIItem
{
    /// <summary>
    ///     道具获得全屏展示界面。
    /// </summary>
    public class UIItemGetOverlayForm : UGuiForm
    {
        [FormerlySerializedAs("itemIcon")] [Header("UI Elements")] [SerializeField]
        private UIItemIcon uiItemIcon;

        [SerializeField] private Text textTitle;
        [SerializeField] private Text textItemName;
        [SerializeField] private Text textClickTip;

        [Header("Effects & Animation")] [SerializeField]
        private CanvasGroup contentCanvasGroup;

        [SerializeField] private GameObject rareSpineAnimGo; // Spine / Particle 特效节点
       // [SerializeField] private int normalSoundId = 10001;
       // [SerializeField] private int rareSoundId = 10002;
        private bool _canClose;

        // private item_config _config;
        private int _count;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _canClose = false;
            if (userData is ItemGetParams param)
            {
               // _config = param.Config;
                _count = param.Count;
            }

            RefreshUI();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
           // _config = null;
            base.OnClose(isShutdown, userData);
        }

        private void RefreshUI()
        {
            /*if (_config == null)
            {
                Close(true);
                return;
            }

            // 1. 设置 Icon 与名称
            if (uiItemIcon != null)
            {
                uiItemIcon.SetData(_config, _count);
            }

            if (textItemName != null)
            {
                textItemName.text = GameEntry.Localization.GetString(_config.NameKey);
                textItemName.color = GetQualityColor(_config.Quality);
            }

            if (textClickTip != null)
            {
                textClickTip.gameObject.SetActive(false);
            }

            // 2. 根据品质区分表现逻辑
            var isRare = _config.Quality == quality_type.epic ||
                         _config.Quality == quality_type.legendary ||
                         _config.Quality == quality_type.mythic;

            if (textTitle != null)
            {
                textTitle.text = GameEntry.Localization.GetString(isRare ? "Item.GetRareTitle" : "Item.GetNormalTitle");
                // 如果是稀有物资，可以使用金色或更亮眼的艺术字表现
                textTitle.color = isRare ? new Color(1f, 0.8f, 0f) : Color.white;
            }

            if (rareSpineAnimGo != null)
            {
                rareSpineAnimGo.SetActive(isRare);
            }

            // 播放对应档次的获得音效
            PlayUISound(isRare ? rareSoundId : normalSoundId);

            // 3. 开始进场动画协程
            StartCoroutine(ShowSequence(isRare));*/
        }

        private IEnumerator ShowSequence(bool isRare)
        {
            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 0f;
            }

            // 逐渐淡入
            var elapsed = 0f;
            var duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                if (contentCanvasGroup != null)
                {
                    contentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                }

                yield return null;
            }

            if (contentCanvasGroup != null)
            {
                contentCanvasGroup.alpha = 1f;
            }

            // 稀有道具增加额外的特效停留等待时长以渲染仪式感
            if (isRare)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }

            // 允许点击屏幕任意位置关闭
            if (textClickTip != null)
            {
                textClickTip.gameObject.SetActive(true);
                textClickTip.text = GameEntry.Localization.GetString("Item.GetClickCloseTip");
            }

            _canClose = true;
        }

        public void OnClickScreen()
        {
            if (_canClose)
            {
                Close();
            }
        }

        // private Color GetQualityColor(quality_type quality)
        // {
        //     switch (quality)
        //     {
        //         case quality_type.common: return Color.white;
        //         case quality_type.uncommon: return new Color(0.3f, 0.8f, 0.3f);
        //         case quality_type.rare: return new Color(0.2f, 0.6f, 1f);
        //         case quality_type.epic: return new Color(0.7f, 0.2f, 0.9f);
        //         case quality_type.legendary: return new Color(1f, 0.6f, 0f);
        //         case quality_type.mythic: return Color.red;
        //         default: return Color.white;
        //     }
        // }
    }

    /// <summary>
    ///     道具获得界面入参。
    /// </summary>
    public class ItemGetParams
    {
        public object Config; // item_config
        public int Count;
    }
}