using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Main;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Main.Editor.Generator.UI.Component;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class HelpFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/HelpForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/setting_background.png";
        private const string ItemBackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/dialog_title_background.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string CircleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/CircleButton.prefab";
        private const string CircleCloseSpritePath = "Assets/Game/Textures/Button/Menu/circle_close.png";
        private const string CategoryBtnNormalPath = "Assets/Game/Textures/Button/Menu/category_btn_normal.png";
        private const string CategoryBtnSelectedPath = "Assets/Game/Textures/Button/Menu/category_btn_selected.png";

        private static void ConfigureTextureAsSprite(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
        }

        [MenuItem("Generator/UI/Form/Create Help Form Prefab")]
        public static void CreateHelpFormPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/System";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "System");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(ItemBackgroundSpritePath);

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            Sprite itemBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ItemBackgroundSpritePath);

            GameObject rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);
            GameObject circleButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CircleButtonPrefabPath);

            // 创建根节点
            GameObject rootGo = CreateRoot();
            HelpForm helpForm = rootGo.AddComponent<HelpForm>();

            // 1. 背景板 (Antique scroll background)
            GameObject bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            RectTransform bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(1200f, 820f);
            Image bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Sliced;
            }
            bgImg.color = Color.white;

            // 2. 标题牌匾 (Title)
            GameObject titleGo = new GameObject("Title");
            titleGo.layer = LayerMask.NameToLayer("UI");
            titleGo.transform.SetParent(bgGo.transform, false);
            RectTransform titleRt = titleGo.AddComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -85f);
            titleRt.sizeDelta = new Vector2(360f, 56f);
            TextMeshProUGUI titleText = titleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) titleText.font = fontAsset;
            titleText.text = "帮助手册";
            titleText.fontSize = 32f;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // 3. 右上角合卷/关闭按钮
            BaseButton closeBtn = null;
            if (circleButtonPrefab != null)
            {
                GameObject closeGo = (GameObject)PrefabUtility.InstantiatePrefab(circleButtonPrefab);
                closeGo.name = "CloseButton";
                closeGo.transform.SetParent(bgGo.transform, false);
                RectTransform closeRt = closeGo.GetComponent<RectTransform>();
                closeRt.anchorMin = new Vector2(1f, 1f);
                closeRt.anchorMax = new Vector2(1f, 1f);
                closeRt.pivot = new Vector2(1f, 1f);
                closeRt.anchoredPosition = new Vector2(9f, 9f);
                closeRt.sizeDelta = new Vector2(90f, 90f);

                var circleBtn = closeGo.GetComponent<CircleButton>();
                if (circleBtn != null)
                {
                    circleBtn.SetTextActive(false);
                    SetPrivateField(circleBtn, "shortcutKey", KeyCode.Escape);
                }
                closeBtn = circleBtn;

                ConfigureTextureAsSprite(CircleCloseSpritePath);
                var closeImage = closeGo.transform.Find("Image")?.GetComponent<Image>();
                if (closeImage != null)
                {
                    Sprite closeSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CircleCloseSpritePath);
                    if (closeSprite != null)
                    {
                        closeImage.sprite = closeSprite;
                    }
                }
            }

            // 4. 左侧目录导航栏 (Left Panel)
            GameObject categoryButtonGroupPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CategoryButtonGroupCreator.PrefabPath);
            if (categoryButtonGroupPrefab == null)
            {
                Debug.LogError($"[HelpFormCreator] CategoryButtonGroup prefab is missing! Please run 'Generator/UI/Form/Create Category Button Group Prefab' first.");
                Object.DestroyImmediate(rootGo);
                return;
            }

            GameObject leftPanelGo = (GameObject)PrefabUtility.InstantiatePrefab(categoryButtonGroupPrefab);
            leftPanelGo.name = "LeftPanel";
            leftPanelGo.transform.SetParent(bgGo.transform, false);
            CategoryButtonGroup categoryButtonGroup = leftPanelGo.GetComponent<CategoryButtonGroup>();

            // 5. 右侧内容区 (Right Panel)
            GameObject rightPanelGo = new GameObject("RightPanel");
            rightPanelGo.layer = LayerMask.NameToLayer("UI");
            rightPanelGo.transform.SetParent(bgGo.transform, false);
            RectTransform rightRt = rightPanelGo.AddComponent<RectTransform>();
            rightRt.anchorMin = new Vector2(1f, 0.5f);
            rightRt.anchorMax = new Vector2(1f, 0.5f);
            rightRt.pivot = new Vector2(1f, 0.5f);
            rightRt.anchoredPosition = new Vector2(-60f, -40f);
            rightRt.sizeDelta = new Vector2(780f, 600f);

            // Right Panel Page Title
            GameObject pageTitleGo = new GameObject("PageTitle");
            pageTitleGo.layer = LayerMask.NameToLayer("UI");
            pageTitleGo.transform.SetParent(rightPanelGo.transform, false);
            RectTransform ptRt = pageTitleGo.AddComponent<RectTransform>();
            ptRt.anchorMin = new Vector2(0f, 1f);
            ptRt.anchorMax = new Vector2(1f, 1f);
            ptRt.pivot = new Vector2(0.5f, 1f);
            ptRt.anchoredPosition = new Vector2(0f, -5f);
            ptRt.sizeDelta = new Vector2(0f, 40f);
            TextMeshProUGUI pageTitleTxt = pageTitleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) pageTitleTxt.font = fontAsset;
            pageTitleTxt.text = "游戏机制概览";
            pageTitleTxt.fontSize = 24f;
            pageTitleTxt.alignment = TextAlignmentOptions.Center;
            pageTitleTxt.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // Right Scroll Rect (for help items)
            GameObject scrollContentGo = new GameObject("ContentScrollView");
            scrollContentGo.layer = LayerMask.NameToLayer("UI");
            scrollContentGo.transform.SetParent(rightPanelGo.transform, false);
            RectTransform scrollContentRt = scrollContentGo.AddComponent<RectTransform>();
            scrollContentRt.anchorMin = Vector2.zero;
            scrollContentRt.anchorMax = Vector2.one;
            scrollContentRt.anchoredPosition = new Vector2(0f, 27.5f);
            scrollContentRt.sizeDelta = new Vector2(0f, -145f);

            ScrollRect contentScroll = scrollContentGo.AddComponent<ScrollRect>();
            contentScroll.horizontal = false;
            contentScroll.vertical = true;
            contentScroll.scrollSensitivity = 25f;

            // Viewport
            GameObject viewportItemGo = new GameObject("Viewport");
            viewportItemGo.layer = LayerMask.NameToLayer("UI");
            viewportItemGo.transform.SetParent(scrollContentGo.transform, false);
            RectTransform viewportItemRt = viewportItemGo.AddComponent<RectTransform>();
            viewportItemRt.anchorMin = Vector2.zero;
            viewportItemRt.anchorMax = Vector2.one;
            viewportItemRt.sizeDelta = Vector2.zero;
            var viewportImage = viewportItemGo.AddComponent<Image>();
            viewportImage.color = new Color(0f, 0f, 0f, 0f);
            viewportImage.raycastTarget = true;
            viewportItemGo.AddComponent<RectMask2D>();
            contentScroll.viewport = viewportItemRt;

            // Content
            GameObject containerItemGo = new GameObject("Content");
            containerItemGo.layer = LayerMask.NameToLayer("UI");
            containerItemGo.transform.SetParent(viewportItemGo.transform, false);
            RectTransform containerItemRt = containerItemGo.AddComponent<RectTransform>();
            containerItemRt.anchorMin = new Vector2(0f, 1f);
            containerItemRt.anchorMax = new Vector2(1f, 1f);
            containerItemRt.pivot = new Vector2(0.5f, 1f);
            containerItemRt.sizeDelta = new Vector2(0f, 0f);

            VerticalLayoutGroup vlgItem = containerItemGo.AddComponent<VerticalLayoutGroup>();
            vlgItem.spacing = 30f;
            vlgItem.childAlignment = TextAnchor.UpperCenter;
            vlgItem.childControlHeight = true;
            vlgItem.childControlWidth = false;
            vlgItem.childForceExpandHeight = false;
            vlgItem.childForceExpandWidth = false;

            ContentSizeFitter csfItem = containerItemGo.AddComponent<ContentSizeFitter>();
            csfItem.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentScroll.content = containerItemRt;

            GameObject helpItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(HelpItemCreator.PrefabPath);
            if (helpItemPrefab == null)
            {
                Debug.LogError($"[HelpFormCreator] HelpItem prefab is missing! Please run 'Generator/UI/Component/Create Help Item Prefab' first.");
                Object.DestroyImmediate(rootGo);
                return;
            }
            HelpItem helpItemComp = helpItemPrefab.GetComponent<HelpItem>();

            // 6. Bottom Pager Controls
            GameObject bottomPagerGo = new GameObject("BottomPager");
            bottomPagerGo.layer = LayerMask.NameToLayer("UI");
            bottomPagerGo.transform.SetParent(rightPanelGo.transform, false);
            RectTransform pagerRt = bottomPagerGo.AddComponent<RectTransform>();
            pagerRt.anchorMin = new Vector2(0.5f, 0f);
            pagerRt.anchorMax = new Vector2(0.5f, 0f);
            pagerRt.pivot = new Vector2(0.5f, 0f);
            pagerRt.anchoredPosition = new Vector2(0f, 10f);
            pagerRt.sizeDelta = new Vector2(780f, 80f);

            Sprite pagerBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Button/Menu/help_pager_button.png");

            BaseButton prevPageBtn = null;
            if (rectButtonPrefab != null)
            {
                GameObject prevGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                prevGo.name = "PrevButton";
                prevGo.transform.SetParent(bottomPagerGo.transform, false);
                RectTransform prevRt = prevGo.GetComponent<RectTransform>();
                prevRt.anchorMin = new Vector2(0f, 0.5f);
                prevRt.anchorMax = new Vector2(0f, 0.5f);
                prevRt.pivot = new Vector2(0f, 0.5f);
                prevRt.anchoredPosition = new Vector2(10f, 0f);
                prevRt.sizeDelta = new Vector2(250f, 70f);

                var prevImg = prevGo.transform.Find("Image")?.GetComponent<Image>();
                if (prevImg != null && pagerBtnSprite != null)
                {
                    prevImg.sprite = pagerBtnSprite;
                }

                prevPageBtn = prevGo.GetComponent<BaseButton>();
                SetPrivateField(prevPageBtn, "clickSoundId", 100011);
                var prevTxt = prevGo.GetComponentInChildren<TMP_Text>();
                if (prevTxt != null) prevTxt.text = "HelpForm.Prev";
            }

            BaseButton nextPageBtn = null;
            if (rectButtonPrefab != null)
            {
                GameObject nextGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                nextGo.name = "NextButton";
                nextGo.transform.SetParent(bottomPagerGo.transform, false);
                RectTransform nextRt = nextGo.GetComponent<RectTransform>();
                nextRt.anchorMin = new Vector2(1f, 0.5f);
                nextRt.anchorMax = new Vector2(1f, 0.5f);
                nextRt.pivot = new Vector2(1f, 0.5f);
                nextRt.anchoredPosition = new Vector2(-10f, 0f);
                nextRt.sizeDelta = new Vector2(250f, 70f);

                var nextImg = nextGo.transform.Find("Image")?.GetComponent<Image>();
                if (nextImg != null && pagerBtnSprite != null)
                {
                    nextImg.sprite = pagerBtnSprite;
                }

                nextPageBtn = nextGo.GetComponent<BaseButton>();
                SetPrivateField(nextPageBtn, "clickSoundId", 100011);
                var nextTxt = nextGo.GetComponentInChildren<TMP_Text>();
                if (nextTxt != null) nextTxt.text = "HelpForm.Next";
            }

            GameObject indicatorGo = new GameObject("PageIndicator");
            indicatorGo.layer = LayerMask.NameToLayer("UI");
            indicatorGo.transform.SetParent(bottomPagerGo.transform, false);
            RectTransform indicatorRt = indicatorGo.AddComponent<RectTransform>();
            indicatorRt.anchorMin = new Vector2(0.5f, 0.5f);
            indicatorRt.anchorMax = new Vector2(0.5f, 0.5f);
            indicatorRt.pivot = new Vector2(0.5f, 0.5f);
            indicatorRt.anchoredPosition = Vector2.zero;
            indicatorRt.sizeDelta = new Vector2(100f, 30f);
            TextMeshProUGUI indicatorTxt = indicatorGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) indicatorTxt.font = fontAsset;
            indicatorTxt.text = "1 / 5";
            indicatorTxt.fontSize = 20f;
            indicatorTxt.alignment = TextAlignmentOptions.Center;
            indicatorTxt.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // Reflection-based private field bindings
            SetPrivateField(helpForm, "closeButton", closeBtn);
            SetPrivateField(helpForm, "titleText", titleText);
            SetPrivateField(helpForm, "categoryButtonGroup", categoryButtonGroup);
            SetPrivateField(helpForm, "pageTitleText", pageTitleTxt);
            SetPrivateField(helpForm, "itemContainer", containerItemRt);
            SetPrivateField(helpForm, "itemTemplate", helpItemComp);
            SetPrivateField(helpForm, "prevPageButton", prevPageBtn);
            SetPrivateField(helpForm, "nextPageButton", nextPageBtn);
            SetPrivateField(helpForm, "pageIndicatorText", indicatorTxt);

            // Save Prefab
            SavePrefab(rootGo);
        }

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("HelpForm");
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            Canvas canvas = rootGo.AddComponent<Canvas>();
            canvas.vertexColorAlwaysGammaSpace = true;
            rootGo.AddComponent<CanvasGroup>();
            rootGo.AddComponent<GraphicRaycaster>();
            return rootGo;
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            for (var type = target.GetType(); type != null; type = type.BaseType)
            {
                var field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                if (field != null)
                {
                    field.SetValue(target, value);
                    return;
                }
            }
            Debug.LogError($"[HelpFormCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }

        private static void SavePrefab(GameObject rootGo)
        {
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"HelpForm Prefab successfully generated at: {PrefabPath}");
        }
    }
}
