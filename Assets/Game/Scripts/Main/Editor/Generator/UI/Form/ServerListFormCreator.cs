// 创建时间：2026-07-27
// 修改时间：2026-07-27

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Menu;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Main.Editor.Generator.UI.Component;
using Game.Scripts.Main.Runtime.UI.UICommon;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class ServerListFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/ServerListForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/setting_background.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string CircleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/CircleButton.prefab";
        private const string CircleCloseSpritePath = "Assets/Game/Textures/Button/Menu/circle_close.png";
        private const string ConfirmButtonSpritePath = "Assets/Game/Textures/Button/Menu/confirm_button_default.png";

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

        [MenuItem("Generator/UI/Form/Create Server List Form Prefab")]
        public static void CreateServerListFormPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/UIMenu";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "UIMenu");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(ConfirmButtonSpritePath);

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            GameObject rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);
            GameObject circleButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CircleButtonPrefabPath);
            GameObject itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ServerListItemCreator.PrefabPath);
            GameObject categoryButtonGroupPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CategoryButtonGroupCreator.PrefabPath);

            if (itemPrefab == null)
            {
                Debug.LogError("[ServerListFormCreator] ServerListItem prefab is missing! Please run 'Generator/UI/Component/Create Server List Item Prefab' first.");
                return;
            }

            if (categoryButtonGroupPrefab == null)
            {
                Debug.LogError("[ServerListFormCreator] CategoryButtonGroup prefab is missing! Please run 'Generator/UI/Component/Create Category Button Group Prefab' first.");
                return;
            }

            // Create root
            GameObject rootGo = CreateRoot();
            ServerListForm form = rootGo.AddComponent<ServerListForm>();

            // 1. Background Scroll Panel
            GameObject bgGo = new GameObject("Background", typeof(RectTransform));
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            RectTransform bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(1200f, 880f);
            Image bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Sliced;
            }
            bgImg.color = Color.white;

            // 2. Title
            GameObject titleGo = new GameObject("Title", typeof(RectTransform));
            titleGo.layer = LayerMask.NameToLayer("UI");
            titleGo.transform.SetParent(bgGo.transform, false);
            RectTransform titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -85f);
            titleRt.sizeDelta = new Vector2(360f, 56f);
            TextMeshProUGUI titleText = titleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) titleText.font = fontAsset;
            titleText.text = "选择割据之地";
            titleText.fontSize = 32f;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // 3. Close Button
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
                if (closeBtn != null)
                {
                    SetPrivateField(closeBtn, "clickSoundId", 100017);
                }

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

            // 4. Left Zone Tab Group
            GameObject leftPanelGo = (GameObject)PrefabUtility.InstantiatePrefab(categoryButtonGroupPrefab);
            leftPanelGo.name = "LeftPanel";
            leftPanelGo.transform.SetParent(bgGo.transform, false);
            RectTransform leftRt = leftPanelGo.GetComponent<RectTransform>();
            leftRt.anchorMin = new Vector2(0f, 0.5f);
            leftRt.anchorMax = new Vector2(0f, 0.5f);
            leftRt.pivot = new Vector2(0f, 0.5f);
            leftRt.anchoredPosition = new Vector2(60f, -20f);
            leftRt.sizeDelta = new Vector2(280f, 600f);
            CategoryButtonGroup categoryButtonGroup = leftPanelGo.GetComponent<CategoryButtonGroup>();
            if (categoryButtonGroup != null)
            {
                SetPrivateField(categoryButtonGroup, "tabSwitchSoundId", 100022);
            }

            // 5. Right Server Grid Panel (ScrollRect)
            GameObject rightPanelGo = new GameObject("RightPanel", typeof(RectTransform));
            rightPanelGo.layer = LayerMask.NameToLayer("UI");
            rightPanelGo.transform.SetParent(bgGo.transform, false);
            RectTransform rightRt = rightPanelGo.GetComponent<RectTransform>();
            rightRt.anchorMin = new Vector2(1f, 0.5f);
            rightRt.anchorMax = new Vector2(1f, 0.5f);
            rightRt.pivot = new Vector2(1f, 0.5f);
            rightRt.anchoredPosition = new Vector2(-60f, -20f);
            rightRt.sizeDelta = new Vector2(780f, 600f);

            ScrollRect scrollRect = rightPanelGo.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 25f;

            // Viewport
            GameObject viewportGo = new GameObject("Viewport", typeof(RectTransform));
            viewportGo.layer = LayerMask.NameToLayer("UI");
            viewportGo.transform.SetParent(rightPanelGo.transform, false);
            RectTransform viewportRt = viewportGo.GetComponent<RectTransform>();
            viewportRt.anchorMin = Vector2.zero;
            viewportRt.anchorMax = Vector2.one;
            viewportRt.sizeDelta = Vector2.zero;
            var viewportImage = viewportGo.AddComponent<Image>();
            viewportImage.color = new Color(0f, 0f, 0f, 0f);
            viewportImage.raycastTarget = true;
            viewportGo.AddComponent<RectMask2D>();
            scrollRect.viewport = viewportRt;

            // Content Container
            GameObject containerGo = new GameObject("Content", typeof(RectTransform));
            containerGo.layer = LayerMask.NameToLayer("UI");
            containerGo.transform.SetParent(viewportGo.transform, false);
            RectTransform containerRt = containerGo.GetComponent<RectTransform>();
            containerRt.anchorMin = new Vector2(0f, 1f);
            containerRt.anchorMax = new Vector2(1f, 1f);
            containerRt.pivot = new Vector2(0.5f, 1f);
            containerRt.sizeDelta = new Vector2(0f, 0f);

            GridLayoutGroup grid = containerGo.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(340f, 130f);
            grid.spacing = new Vector2(30f, 20f);
            grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.childAlignment = TextAnchor.UpperLeft;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 2;

            ContentSizeFitter csf = containerGo.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = containerRt;

            // Add server card template inside Content (hidden by default)
            GameObject itemTemplateInst = (GameObject)PrefabUtility.InstantiatePrefab(itemPrefab);
            itemTemplateInst.name = "ServerListItemTemplate";
            itemTemplateInst.transform.SetParent(containerRt, false);
            ServerListItem serverCardTemplate = itemTemplateInst.GetComponent<ServerListItem>();

            // 6. Bottom Selected Server Info & Confirm Button
            GameObject bottomPanelGo = new GameObject("BottomPanel", typeof(RectTransform));
            bottomPanelGo.layer = LayerMask.NameToLayer("UI");
            bottomPanelGo.transform.SetParent(bgGo.transform, false);
            RectTransform bottomRt = bottomPanelGo.GetComponent<RectTransform>();
            bottomRt.anchorMin = new Vector2(0.5f, 0f);
            bottomRt.anchorMax = new Vector2(0.5f, 0f);
            bottomRt.pivot = new Vector2(0.5f, 0f);
            bottomRt.anchoredPosition = new Vector2(0f, 40f);
            bottomRt.sizeDelta = new Vector2(1080f, 80f);

            // Selected Server Text
            GameObject selTextGo = new GameObject("SelectedServerText", typeof(RectTransform));
            selTextGo.layer = LayerMask.NameToLayer("UI");
            selTextGo.transform.SetParent(bottomPanelGo.transform, false);
            RectTransform selTextRt = selTextGo.GetComponent<RectTransform>();
            selTextRt.anchorMin = new Vector2(0f, 0.5f);
            selTextRt.anchorMax = new Vector2(0.7f, 0.5f);
            selTextRt.pivot = new Vector2(0f, 0.5f);
            selTextRt.anchoredPosition = new Vector2(20f, 0f);
            selTextRt.sizeDelta = new Vector2(0f, 50f);
            TextMeshProUGUI selText = selTextGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) selText.font = fontAsset;
            selText.text = "请选择割据之地...";
            selText.fontSize = 22f;
            selText.alignment = TextAlignmentOptions.Left;
            selText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

             // Confirm Button
            BaseButton confirmBtn = null;
            if (rectButtonPrefab != null)
            {
                GameObject confirmGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                confirmGo.name = "ConfirmButton";
                confirmGo.transform.SetParent(bottomPanelGo.transform, false);
                RectTransform confirmRt = confirmGo.GetComponent<RectTransform>();
                confirmRt.anchorMin = new Vector2(1f, 0.5f);
                confirmRt.anchorMax = new Vector2(1f, 0.5f);
                confirmRt.pivot = new Vector2(1f, 0.5f);
                confirmRt.anchoredPosition = new Vector2(-20f, 0f);
                confirmRt.sizeDelta = new Vector2(250f, 70f);

                confirmBtn = confirmGo.GetComponent<BaseButton>();
                if (confirmBtn != null)
                {
                    SetPrivateField(confirmBtn, "clickSoundId", 100024);
                    SetPrivateField(confirmBtn, "shortcutKey", KeyCode.Return);
                }
                var btnTxt = confirmGo.GetComponentInChildren<TMP_Text>();
                if (btnTxt != null)
                {
                    btnTxt.text = "确定立足";
                }
                var btnImg = confirmGo.transform.Find("Image")?.GetComponent<Image>();
                if (btnImg != null)
                {
                    Sprite confirmSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ConfirmButtonSpritePath);
                    if (confirmSprite != null)
                    {
                        btnImg.sprite = confirmSprite;
                        btnImg.type = Image.Type.Simple;
                    }
                }
            }

            // Bind Form Fields via reflection
            SetPrivateField(form, "titleText", titleText);
            SetPrivateField(form, "zoneTabList", categoryButtonGroup);
            SetPrivateField(form, "serverCardList", scrollRect);
            SetPrivateField(form, "serverCardContainer", containerRt);
            SetPrivateField(form, "serverCardTemplate", serverCardTemplate);
            SetPrivateField(form, "selectedServerText", selText);
            SetPrivateField(form, "confirmButton", confirmBtn);
            SetPrivateField(form, "closeButton", closeBtn);

            // Save Prefab
            SavePrefab(rootGo);
        }

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("ServerListForm", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.GetComponent<RectTransform>();
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
            Debug.LogError($"[ServerListFormCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }

        private static void SavePrefab(GameObject rootGo)
        {
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"ServerListForm Prefab successfully generated at: {PrefabPath}");
        }
    }
}
