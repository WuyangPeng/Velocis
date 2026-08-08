// 创建时间：2026-07-24
// 修改时间：2026-07-24

using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Main;
using Game.Scripts.Main.Editor.Generator.UI.Component;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class AnnouncementFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/AnnouncementForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/setting_background.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string ItemNormalSpritePath = "Assets/Game/Textures/Button/Menu/category_btn_normal.png";
        private const string ItemSelectedSpritePath = "Assets/Game/Textures/Button/Menu/category_btn_selected.png";

        private static void ConfigureTextureAsSprite(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
        }

        [MenuItem("Generator/UI/Form/Create Announcement Form Prefab")]
        public static void CreateAnnouncementFormPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/System";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "System");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(ItemNormalSpritePath);
            ConfigureTextureAsSprite(ItemSelectedSpritePath);

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            var itemNormalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ItemNormalSpritePath);
            var itemSelectedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ItemSelectedSpritePath);
            var rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);
            var itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AnnouncementItemCreator.PrefabPath);
            if (itemPrefab == null)
            {
                Debug.LogError("[AnnouncementFormCreator] AnnouncementItem prefab is missing! Please run 'Generator/UI/Component/Create Announcement Item Prefab' first.");
                return;
            }

            var detailPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AnnouncementDetailCreator.PrefabPath);
            if (detailPrefab == null)
            {
                Debug.LogError("[AnnouncementFormCreator] AnnouncementDetail prefab is missing! Please run 'Generator/UI/Component/Create Announcement Detail Prefab' first.");
                return;
            }

            var itemComp = itemPrefab.GetComponent<AnnouncementItem>();
            var rootGo = CreateRoot();
            var form = rootGo.AddComponent<AnnouncementForm>();

            // Background — 古风告示牌底板（约 1000x800）
            var bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            var bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(1000f, 800f);
            var bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Sliced;
            }

            bgImg.color = Color.white;

            // Title
            var titleGo = new GameObject("Title");
            titleGo.layer = LayerMask.NameToLayer("UI");
            titleGo.transform.SetParent(bgGo.transform, false);
            var titleRt = titleGo.AddComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -72f);
            titleRt.sizeDelta = new Vector2(360f, 48f);
            var titleText = titleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                titleText.font = fontAsset;
            }

            titleText.text = "AnnouncementForm.Title";
            titleText.fontSize = 30f;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // Left list panel
            var leftGo = new GameObject("LeftPanel");
            leftGo.layer = LayerMask.NameToLayer("UI");
            leftGo.transform.SetParent(bgGo.transform, false);
            var leftRt = leftGo.AddComponent<RectTransform>();
            leftRt.anchorMin = new Vector2(0f, 0.5f);
            leftRt.anchorMax = new Vector2(0f, 0.5f);
            leftRt.pivot = new Vector2(0f, 0.5f);
            leftRt.anchoredPosition = new Vector2(50f, 0f);
            leftRt.sizeDelta = new Vector2(300f, 500f);

            var listScrollGo = new GameObject("AnnouncementList");
            listScrollGo.layer = LayerMask.NameToLayer("UI");
            listScrollGo.transform.SetParent(leftGo.transform, false);
            var listScrollRt = listScrollGo.AddComponent<RectTransform>();
            listScrollRt.anchorMin = Vector2.zero;
            listScrollRt.anchorMax = Vector2.one;
            listScrollRt.sizeDelta = Vector2.zero;
            var listScroll = listScrollGo.AddComponent<ScrollRect>();
            listScroll.horizontal = false;
            listScroll.vertical = true;
            listScroll.scrollSensitivity = 25f;

            var listViewportGo = new GameObject("Viewport");
            listViewportGo.layer = LayerMask.NameToLayer("UI");
            listViewportGo.transform.SetParent(listScrollGo.transform, false);
            var listViewportRt = listViewportGo.AddComponent<RectTransform>();
            listViewportRt.anchorMin = Vector2.zero;
            listViewportRt.anchorMax = Vector2.one;
            listViewportRt.sizeDelta = Vector2.zero;
            var listViewportImg = listViewportGo.AddComponent<Image>();
            listViewportImg.color = new Color(0f, 0f, 0f, 0f);
            listViewportImg.raycastTarget = true;
            listViewportGo.AddComponent<RectMask2D>();
            listScroll.viewport = listViewportRt;

            var listContentGo = new GameObject("Content");
            listContentGo.layer = LayerMask.NameToLayer("UI");
            listContentGo.transform.SetParent(listViewportGo.transform, false);
            var listContentRt = listContentGo.AddComponent<RectTransform>();
            listContentRt.anchorMin = new Vector2(0f, 1f);
            listContentRt.anchorMax = new Vector2(1f, 1f);
            listContentRt.pivot = new Vector2(0.5f, 1f);
            listContentRt.sizeDelta = new Vector2(0f, 0f);
            var listVlg = listContentGo.AddComponent<VerticalLayoutGroup>();
            listVlg.spacing = 10f;
            listVlg.childAlignment = TextAnchor.UpperCenter;
            listVlg.childControlHeight = false;
            listVlg.childControlWidth = false;
            listVlg.childForceExpandHeight = false;
            listVlg.childForceExpandWidth = false;
            var listCsf = listContentGo.AddComponent<ContentSizeFitter>();
            listCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            listScroll.content = listContentRt;

            // Right detail panel (实例化预制体组件)
            var detailGo = (GameObject)PrefabUtility.InstantiatePrefab(detailPrefab);
            detailGo.name = "RightPanel";
            detailGo.transform.SetParent(bgGo.transform, false);
            var detailRt = detailGo.GetComponent<RectTransform>();
            detailRt.anchorMin = new Vector2(1f, 0.5f);
            detailRt.anchorMax = new Vector2(1f, 0.5f);
            detailRt.pivot = new Vector2(1f, 0.5f);
            detailRt.anchoredPosition = new Vector2(-50f, 0f);
            detailRt.sizeDelta = new Vector2(570f, 500f);
            var detailCompInstance = detailGo.GetComponent<AnnouncementDetail>();

            // Close / 已阅 stamp button
            BaseButton closeBtn = null;
            if (rectButtonPrefab != null)
            {
                var closeGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                closeGo.name = "CloseButton";
                closeGo.transform.SetParent(bgGo.transform, false);
                var closeRt = closeGo.GetComponent<RectTransform>();
                closeRt.anchorMin = new Vector2(0.5f, 0f);
                closeRt.anchorMax = new Vector2(0.5f, 0f);
                closeRt.pivot = new Vector2(0.5f, 0f);
                closeRt.anchoredPosition = new Vector2(0f, 60f);
                closeRt.sizeDelta = new Vector2(220f, 64f);
                closeBtn = closeGo.GetComponent<BaseButton>();
                UIEditorCreatorUtility.SetPrivateField(closeBtn, "clickSoundId", 100017);
                UIEditorCreatorUtility.SetPrivateField(closeBtn, "shortcutKey", KeyCode.Escape);
                var closeTxt = closeGo.GetComponentInChildren<TMP_Text>();
                if (closeTxt != null)
                {
                    closeTxt.text = "AnnouncementForm.BtnRead";
                }
            }

            UIEditorCreatorUtility.SetPrivateField(form, "closeButton", closeBtn);
            UIEditorCreatorUtility.SetPrivateField(form, "titleText", titleText);
            UIEditorCreatorUtility.SetPrivateField(form, "listContainer", listContentRt);
            UIEditorCreatorUtility.SetPrivateField(form, "itemTemplate", itemComp);
            UIEditorCreatorUtility.SetPrivateField(form, "itemNormalSprite", itemNormalSprite);
            UIEditorCreatorUtility.SetPrivateField(form, "itemSelectedSprite", itemSelectedSprite);
            UIEditorCreatorUtility.SetPrivateField(form, "detailPanel", detailCompInstance);
            UIEditorCreatorUtility.SetPrivateField(form, "openSoundId", 100015);
            UIEditorCreatorUtility.SetPrivateField(form, "selectSoundId", 100016);
            UIEditorCreatorUtility.SetPrivateField(form, "stampSoundId", 100017);

            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"AnnouncementForm Prefab successfully generated at: {PrefabPath}");
        }

        private static GameObject CreateRoot()
        {
            var rootGo = new GameObject("AnnouncementForm");
            rootGo.layer = LayerMask.NameToLayer("UI");
            var rt = rootGo.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            var canvas = rootGo.AddComponent<Canvas>();
            canvas.vertexColorAlwaysGammaSpace = true;
            rootGo.AddComponent<CanvasGroup>();
            rootGo.AddComponent<GraphicRaycaster>();
            return rootGo;
        }
    }
}
