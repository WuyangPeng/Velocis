using System;
using System.Collections.Generic;
using Celeritas.Config.game;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Config;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework;
using GameFramework.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Main
{
    public class HelpForm : UGuiForm
    {
        // ──────────────────────────────────────────────
        // 序列化字段
        // ──────────────────────────────────────────────
        [SerializeField] private BaseButton closeButton;

        [SerializeField] private TMP_Text titleText;
        [SerializeField] private CategoryButtonGroup categoryButtonGroup;

        [SerializeField] private TMP_Text pageTitleText;
        [SerializeField] private RectTransform itemContainer;
        [SerializeField] private HelpItem itemTemplate;

        [SerializeField] private BaseButton prevPageButton;
        [SerializeField] private BaseButton nextPageButton;
        [SerializeField] private TMP_Text pageIndicatorText;

        // ──────────────────────────────────────────────
        // 私有状态与池化缓存
        // ──────────────────────────────────────────────
        private readonly List<HelpCategoryData> _categories = new();
        private readonly List<GameObject> _categoryButtonsPool = new();
        private readonly List<HelpItem> _itemsPool = new();

        private int _currentCategoryIndex;
        private int _currentPageIndex;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            InitHelpData();
            InitUI();

            // 默认选中第一个分类的第一页
            SelectCategory(0);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void InitHelpData()
        {
            _categories.Clear();

            var tables = GameEntry.GameConfig.GetTables();
            if (tables == null ||
                tables.HelpCategoryConfigContainer == null ||
                tables.HelpPageConfigContainer == null ||
                tables.HelpItemConfigContainer == null)
            {
                Log.Error("Config tables or containers are null.");
                CreateFallbackData();
                return;
            }

            var categories = new List<help_category_config>(tables.HelpCategoryConfigContainer.DataList);
            categories.Sort((a, b) => a.Sort.CompareTo(b.Sort));

            var pages = new List<help_page_config>(tables.HelpPageConfigContainer.DataList);
            pages.Sort((a, b) => a.Sort.CompareTo(b.Sort));

            var items = new List<help_item_config>(tables.HelpItemConfigContainer.DataList);
            items.Sort((a, b) => a.Sort.CompareTo(b.Sort));

            foreach (var categoryConfig in categories)
            {
                var categoryData = new HelpCategoryData
                {
                    categoryName = GameEntry.Localization.GetString(categoryConfig.NameKey),
                    iconPath = categoryConfig.IconPath,
                    pages = new List<HelpPageData>()
                };

                foreach (var pageConfig in pages)
                {
                    if (pageConfig.CategoryId == categoryConfig.Id)
                    {
                        var pageData = new HelpPageData
                        {
                            pageTitle = GameEntry.Localization.GetString(pageConfig.TitleKey),
                            items = new List<HelpItemData>()
                        };

                        foreach (var itemConfig in items)
                        {
                            if (itemConfig.PageId == pageConfig.Id)
                            {
                                var itemData = new HelpItemData
                                {
                                    iconPath = itemConfig.IconPath,
                                    title = GameEntry.Localization.GetString(itemConfig.TitleKey),
                                    description = string.Empty,
                                    descFileId = itemConfig.DescFileId
                                };
                                pageData.items.Add(itemData);
                            }
                        }

                        categoryData.pages.Add(pageData);
                    }
                }

                _categories.Add(categoryData);
            }

            if (_categories.Count == 0)
            {
                Log.Warning("No help categories found in configuration.");
                CreateFallbackData();
            }
        }

        private void CreateFallbackData()
        {
            _categories.Clear();
            var fallbackCategory = new HelpCategoryData
            {
                categoryName = GameEntry.Localization.GetString("HelpForm.Category.Beginner"),
                iconPath = string.Empty,
                pages = new List<HelpPageData>
                {
                    new()
                    {
                        pageTitle = GameEntry.Localization.GetString("HelpForm.NoContent"),
                        items = new List<HelpItemData>
                        {
                            new()
                            {
                                iconPath = string.Empty,
                                title = GameEntry.Localization.GetString("HelpForm.LoadFailed"),
                                description = GameEntry.Localization.GetString("HelpForm.LoadFailed"),
                                descFileId = string.Empty
                            }
                        }
                    }
                }
            };
            _categories.Add(fallbackCategory);
        }

        private void InitUI()
        {
            // 绑定关闭按钮
            if (closeButton != null)
            {
                closeButton.OnClick.RemoveAllListeners();
                closeButton.OnClick.AddListener(OnCloseButtonClick);
            }

            // 绑定翻页按钮
            if (prevPageButton != null)
            {
                prevPageButton.OnClick.RemoveAllListeners();
                prevPageButton.OnClick.AddListener(OnPrevPageButtonClick);
            }

            if (nextPageButton != null)
            {
                nextPageButton.OnClick.RemoveAllListeners();
                nextPageButton.OnClick.AddListener(OnNextPageButtonClick);
            }

            if (titleText != null)
            {
                titleText.text = GameEntry.Localization.GetString("HelpForm.Title");
            }

            // 隐藏模板
            if (categoryButtonGroup != null && categoryButtonGroup.CategoryButtonTemplate != null)
            {
                categoryButtonGroup.CategoryButtonTemplate.SetActive(false);
            }

            if (itemTemplate != null)
            {
                itemTemplate.gameObject.SetActive(false);
            }

            // 生成左侧大类目录按钮
            RefreshCategoryButtons();
        }

        private void RefreshCategoryButtons()
        {
            // 回收已存在的目录按钮
            foreach (var btn in _categoryButtonsPool)
            {
                btn.SetActive(false);
            }

            if (categoryButtonGroup == null || categoryButtonGroup.CategoryButtonTemplate == null || categoryButtonGroup.CategoryContainer == null)
            {
                Log.Error("categoryButtonGroup or template or container is null.");
                return;
            }

            for (var i = 0; i < _categories.Count; i++)
            {
                GameObject btnGo;
                if (i < _categoryButtonsPool.Count)
                {
                    btnGo = _categoryButtonsPool[i];
                    btnGo.SetActive(true);
                }
                else
                {
                    btnGo = Instantiate(categoryButtonGroup.CategoryButtonTemplate, categoryButtonGroup.CategoryContainer, false);
                    _categoryButtonsPool.Add(btnGo);
                }

                btnGo.SetActive(true);

                // 绑定文字与图标
                var txt = btnGo.GetComponentInChildren<TMP_Text>();
                if (txt != null)
                {
                    txt.text = _categories[i].categoryName;
                }

                // 设置左侧分类图标（若需要）
                var iconImg = btnGo.transform.Find("Icon")?.GetComponent<Image>();
                if (iconImg != null && !string.IsNullOrEmpty(_categories[i].iconPath))
                {
                    // 暂时硬编码或从资源加载，如果有对应美术素材的话
                    // iconImg.sprite = ...
                }

                var index = i;
                var baseBtn = btnGo.GetComponent<BaseButton>();
                if (baseBtn != null)
                {
                    baseBtn.OnClick.RemoveAllListeners();
                    baseBtn.OnClick.AddListener(() =>
                    {
                        if (categoryButtonGroup != null)
                        {
                            categoryButtonGroup.PlayTabSwitchSound();
                        }

                        SelectCategory(index);
                    });
                }
            }
        }

        private void SelectCategory(int categoryIndex)
        {
            _currentCategoryIndex = Mathf.Clamp(categoryIndex, 0, _categories.Count - 1);
            _currentPageIndex = 0;

            // 更新左侧按钮的选中视觉效果
            for (var i = 0; i < _categoryButtonsPool.Count; i++)
            {
                var btnImg = _categoryButtonsPool[i].transform.Find("Image")?.GetComponent<Image>();
                if (btnImg != null && categoryButtonGroup != null)
                {
                    btnImg.sprite = i == _currentCategoryIndex ? categoryButtonGroup.CategorySelectedSprite : categoryButtonGroup.CategoryNormalSprite;
                }

                var btnTxt = _categoryButtonsPool[i].transform.Find("Text")?.GetComponent<TMP_Text>();
                if (btnTxt != null)
                {
                    btnTxt.color = i == _currentCategoryIndex ? new Color(0.17f, 0.12f, 0.08f, 1f) : Color.white;
                }
            }

            RefreshPage();
        }

        private void RefreshPage()
        {
            var category = _categories[_currentCategoryIndex];
            if (category.pages.Count == 0)
            {
                pageTitleText.text = GameEntry.Localization.GetString("HelpForm.NoContent");
                pageIndicatorText.text = "0 / 0";
                ClearRightItems();
                return;
            }

            _currentPageIndex = Mathf.Clamp(_currentPageIndex, 0, category.pages.Count - 1);
            var page = category.pages[_currentPageIndex];

            // 设置标题与页码显示
            if (pageTitleText != null)
            {
                pageTitleText.text = page.pageTitle;
            }

            if (pageIndicatorText != null)
            {
                pageIndicatorText.text = GameEntry.Localization.GetString("HelpForm.PageFormat", _currentPageIndex + 1, category.pages.Count);
            }

            // 刷新翻页按钮显示状态
            if (prevPageButton != null)
            {
                prevPageButton.gameObject.SetActive(_currentPageIndex > 0);
            }

            if (nextPageButton != null)
            {
                nextPageButton.gameObject.SetActive(_currentPageIndex < category.pages.Count - 1);
            }

            // 刷新右侧条目列表
            ClearRightItems();
            if (itemContainer != null)
            {
                itemContainer.anchoredPosition = new Vector2(itemContainer.anchoredPosition.x, 0f);
            }

            for (var i = 0; i < page.items.Count; i++)
            {
                var itemData = page.items[i];
                HelpItem itemGo;
                if (i < _itemsPool.Count)
                {
                    itemGo = _itemsPool[i];
                    itemGo.gameObject.SetActive(true);
                }
                else
                {
                    itemGo = Instantiate(itemTemplate, itemContainer, false);
                    _itemsPool.Add(itemGo);
                }

                itemGo.gameObject.SetActive(true);

                // 绑定标题
                if (itemGo.TitleText != null)
                {
                    itemGo.TitleText.text = itemData.title;
                }

                if (itemGo.DescriptionText != null)
                {
                    if (!string.IsNullOrEmpty(itemData.description))
                    {
                        itemGo.DescriptionText.text = itemData.description;
                    }
                    else
                    {
                        itemGo.DescriptionText.text = GameEntry.Localization.GetString("HelpForm.NoContent");
                        LoadDescription(itemData, itemGo.DescriptionText, itemGo);
                    }
                }

                // 绑定小图标
                if (itemGo.IconImage != null)
                {
                    if (!string.IsNullOrEmpty(itemData.iconPath))
                    {
                        itemGo.IconImage.gameObject.SetActive(true);
                        GameEntry.Resource.LoadAsset(itemData.iconPath, typeof(Sprite), new LoadAssetCallbacks((assetName, asset, duration, userData) =>
                            {
                                if (itemGo.IconImage != null && asset != null)
                                {
                                    itemGo.IconImage.sprite = asset as Sprite;
                                }
                            }
                        ));
                    }
                    else
                    {
                        itemGo.IconImage.gameObject.SetActive(false);
                    }
                }

                ForceRebuildLayout(itemGo);
            }
        }

        private void ForceRebuildLayout(HelpItem itemGo)
        {
            if (itemGo == null)
            {
                return;
            }

            var itemRt = itemGo.GetComponent<RectTransform>();
            var textContainerRt = itemGo.transform.Find("TextContainer") as RectTransform;

            Canvas.ForceUpdateCanvases();

            if (textContainerRt != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(textContainerRt);
            }

            if (itemRt != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(itemRt);
            }

            if (itemContainer != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(itemContainer);
            }
        }

        private void LoadDescription(HelpItemData itemData, TMP_Text descTxt, HelpItem itemGo)
        {
            if (string.IsNullOrEmpty(itemData.descFileId))
            {
                return;
            }

            var assetPath = Utility.Text.Format("Assets/Game/Localization/{0}/Help/{1}.txt", GameEntry.Localization.Language.ToString(), itemData.descFileId);
            GameEntry.Resource.LoadAsset(assetPath, typeof(TextAsset), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    var textAsset = asset as TextAsset;
                    if (textAsset != null)
                    {
                        itemData.description = textAsset.text;
                        if (descTxt != null)
                        {
                            descTxt.text = itemData.description;
                            ForceRebuildLayout(itemGo);
                        }
                    }
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Warning("Load help text file failed: {0}, error: {1}", assetName, errorMessage);
                    if (descTxt != null)
                    {
                        descTxt.text = GameEntry.Localization.GetString("HelpForm.LoadFailed");
                        ForceRebuildLayout(itemGo);
                    }
                }
            ));
        }

        private void ClearRightItems()
        {
            foreach (var item in _itemsPool)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void OnPrevPageButtonClick()
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                RefreshPage();
            }
        }

        private void OnNextPageButtonClick()
        {
            var category = _categories[_currentCategoryIndex];
            if (_currentPageIndex < category.pages.Count - 1)
            {
                _currentPageIndex++;
                RefreshPage();
            }
        }

        private void OnCloseButtonClick()
        {
            Close();
        }

        [Serializable]
        public class HelpItemData
        {
            public string iconPath;
            public string title;
            public string description;
            public string descFileId;
        }

        [Serializable]
        public class HelpPageData
        {
            public string pageTitle;
            public List<HelpItemData> items = new();
        }

        [Serializable]
        public class HelpCategoryData
        {
            public string categoryName;
            public string iconPath;
            public List<HelpPageData> pages = new();
        }
    }
}