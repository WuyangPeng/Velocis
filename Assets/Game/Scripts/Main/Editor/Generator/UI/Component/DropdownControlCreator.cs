using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixBusiness.Tools.Dropdown;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class DropdownControlCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/DropdownControl.prefab";
        private const string DropdownBackgroundSpritePath = "Assets/Game/Textures/Dropdown/Menu/dropdown_background.png";
        private const string DropdownArrowSpritePath = "Assets/Game/Textures/Dropdown/Menu/dropdown_arrow.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        private const float ControlGroupWidth = 620f;
        private const float ControlLabelWidth = 120f;
        private const float ControlLabelFontSize = 22f;
        private const float ControlValueFontSize = 22f;

        private const float DropdownItemHeight = 30f;

        // Template / Viewport 上下内边距，需避开九宫格金边（运行时由 DropdownArrowRotator 补偿列表总高）
        private const float TemplatePaddingTop = 15f;
        private const float TemplatePaddingBottom = 15f;

        private const float TemplatePaddingHorizontal = 30f;

        // Content 初始高度必须与 Item 一致，否则 TMP 会错算选项总高
        private const int TemplateVisibleItemCount = 5;

        private const float TemplateHeight = DropdownItemHeight * TemplateVisibleItemCount
                                             + TemplatePaddingTop + TemplatePaddingBottom;

        private static void ConfigureTextureAsSprite(string path, Vector4 border = default)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                var changed = false;
                if (importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    changed = true;
                }

                if (importer.spriteBorder != border)
                {
                    importer.spriteBorder = border;
                    changed = true;
                }

                if (changed)
                {
                    importer.SaveAndReimport();
                }
            }
        }

        [MenuItem("Generator/UI/Component/Create Dropdown Control Prefab")]
        public static void CreateDropdownControlPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Game/UI"))
                {
                    AssetDatabase.CreateFolder("Assets/Game", "UI");
                }

                if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms"))
                {
                    AssetDatabase.CreateFolder("Assets/Game/UI", "UIForms");
                }

                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            // 九宫格边框设置：左/右边留出50像素保护云纹拐角，上/下留出20像素
            ConfigureTextureAsSprite(DropdownBackgroundSpritePath, new Vector4(50f, 20f, 50f, 20f));
            ConfigureTextureAsSprite(DropdownArrowSpritePath);

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var dropdownBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(DropdownBackgroundSpritePath);
            var dropdownArrowSprite = AssetDatabase.LoadAssetAtPath<Sprite>(DropdownArrowSpritePath);

            var rootGo = new GameObject("DropdownControlGroup");
            rootGo.layer = LayerMask.NameToLayer("UI");
            var rt = rootGo.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(ControlGroupWidth, 52f);

            var dropdownControl = rootGo.AddComponent<DropdownControl>();

            // 1. 标签文本
            var labelGo = new GameObject("Label");
            labelGo.layer = LayerMask.NameToLayer("UI");
            labelGo.transform.SetParent(rootGo.transform, false);
            var labelRt = labelGo.AddComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 0.5f);
            labelRt.anchorMax = new Vector2(0f, 0.5f);
            labelRt.pivot = new Vector2(0f, 0.5f);
            labelRt.anchoredPosition = new Vector2(10f, 0f);
            labelRt.sizeDelta = new Vector2(ControlLabelWidth, 36f);
            var labelTxt = labelGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                labelTxt.font = fontAsset;
            }

            labelTxt.text = "DropdownLabel";
            labelTxt.fontSize = ControlLabelFontSize;
            labelTxt.alignment = TextAlignmentOptions.Left;
            labelTxt.color = Color.white;

            // 2. 下拉框组件
            var dropdownGo = new GameObject("Dropdown");
            dropdownGo.layer = LayerMask.NameToLayer("UI");
            dropdownGo.transform.SetParent(rootGo.transform, false);
            var dropdownRt = dropdownGo.AddComponent<RectTransform>();
            dropdownRt.anchorMin = new Vector2(0f, 0.5f);
            dropdownRt.anchorMax = new Vector2(1f, 0.5f);
            dropdownRt.pivot = new Vector2(0f, 0.5f);
            dropdownRt.anchoredPosition = new Vector2(200f, 0f);
            dropdownRt.sizeDelta = new Vector2(-210f, 44f);

            var dropdownImg = dropdownGo.AddComponent<Image>();
            if (dropdownBgSprite != null)
            {
                dropdownImg.sprite = dropdownBgSprite;
                dropdownImg.type = Image.Type.Sliced;
                dropdownImg.color = Color.white;
            }
            else
            {
                dropdownImg.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);
            }

            var dropdown = dropdownGo.AddComponent<TMP_Dropdown>();
            var arrowRotator = dropdownGo.AddComponent<DropdownArrowRotator>();

            // 创建悬停金光粒子和闪烁模板（挂载在拥有 UI 边框和交互事件的 Dropdown 物体下）
            UIEditorCreatorUtility.CreateHoverParticleLayer(dropdownGo.transform, UIEditorCreatorUtility.DefaultHoverParticleExpand);
            var sparkleTemplateGo = UIEditorCreatorUtility.CreateSparkleTemplate(dropdownGo.transform, UIEditorCreatorUtility.SparkleTemplateSize);

            // 3. 箭头图标
            var arrowGo = new GameObject("Arrow");
            arrowGo.layer = LayerMask.NameToLayer("UI");
            arrowGo.transform.SetParent(dropdownGo.transform, false);
            var arrowRt = arrowGo.AddComponent<RectTransform>();
            arrowRt.anchorMin = new Vector2(1f, 0.5f);
            arrowRt.anchorMax = new Vector2(1f, 0.5f);
            arrowRt.pivot = new Vector2(0.5f, 0.5f);
            arrowRt.anchoredPosition = new Vector2(-25f, 0f);
            arrowRt.sizeDelta = new Vector2(20f, 20f);
            var arrowImg = arrowGo.AddComponent<Image>();
            if (dropdownArrowSprite != null)
            {
                arrowImg.sprite = dropdownArrowSprite;
                arrowImg.color = Color.white;
            }
            else
            {
                arrowImg.color = Color.white;
            }

            // 4. 文本显示
            var textGo = new GameObject("Label");
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(dropdownGo.transform, false);
            var textRt = textGo.AddComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = new Vector2(45f, 0f);
            textRt.offsetMax = new Vector2(-45f, 0f);
            var txt = textGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                txt.font = fontAsset;
            }

            txt.fontSize = 26f;
            txt.fontStyle = FontStyles.Bold;
            txt.alignment = TextAlignmentOptions.Center;
            txt.color = new Color(0.4f, 0.8f, 1f, 1f);
            txt.outlineColor = new Color32(0, 0, 0, 255);
            txt.outlineWidth = 0.25f;

            dropdown.captionText = txt;

            // 5. Template (用于列表显示)
            var templateGo = new GameObject("Template");
            templateGo.layer = LayerMask.NameToLayer("UI");
            templateGo.transform.SetParent(dropdownGo.transform, false);
            templateGo.SetActive(false);
            var templateRt = templateGo.AddComponent<RectTransform>();
            templateRt.anchorMin = new Vector2(0f, 0f);
            templateRt.anchorMax = new Vector2(1f, 0f);
            templateRt.pivot = new Vector2(0.5f, 1f);
            templateRt.sizeDelta = new Vector2(0f, TemplateHeight);
            templateRt.anchoredPosition = new Vector2(0f, -2f);
            var scrollRect = templateGo.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            var tempImg = templateGo.AddComponent<Image>();
            if (dropdownBgSprite != null)
            {
                tempImg.sprite = dropdownBgSprite;
                tempImg.type = Image.Type.Sliced;
                tempImg.color = new Color(0.5f, 0.5f, 0.5f, 1f); // 压暗背景颜色提高对比度
            }
            else
            {
                tempImg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            }

            // Viewport：左右/上下内边距避开九宫格金边。
            // TMP 收高度时不算上下 inset，由 DropdownArrowRotator 在展开时补回。
            var viewportGo = new GameObject("Viewport");
            viewportGo.layer = LayerMask.NameToLayer("UI");
            viewportGo.transform.SetParent(templateGo.transform, false);
            var viewportRt = viewportGo.AddComponent<RectTransform>();
            viewportRt.anchorMin = Vector2.zero;
            viewportRt.anchorMax = Vector2.one;
            viewportRt.offsetMin = new Vector2(TemplatePaddingHorizontal, TemplatePaddingBottom);
            viewportRt.offsetMax = new Vector2(-TemplatePaddingHorizontal, -TemplatePaddingTop);
            viewportGo.AddComponent<RectMask2D>();
            scrollRect.viewport = viewportRt;

            // Content（高度必须与 Item 一致）
            var contentGo = new GameObject("Content");
            contentGo.layer = LayerMask.NameToLayer("UI");
            contentGo.transform.SetParent(viewportGo.transform, false);
            var contentRt = contentGo.AddComponent<RectTransform>();
            contentRt.anchorMin = new Vector2(0f, 1f);
            contentRt.anchorMax = new Vector2(1f, 1f);
            contentRt.pivot = new Vector2(0.5f, 1f);
            contentRt.sizeDelta = new Vector2(0f, DropdownItemHeight);
            scrollRect.content = contentRt;

            // Item
            var itemGo = new GameObject("Item");
            itemGo.layer = LayerMask.NameToLayer("UI");
            itemGo.transform.SetParent(contentGo.transform, false);
            var itemRt = itemGo.AddComponent<RectTransform>();
            itemRt.anchorMin = new Vector2(0f, 0.5f);
            itemRt.anchorMax = new Vector2(1f, 0.5f);
            itemRt.sizeDelta = new Vector2(0f, DropdownItemHeight);
            var itemToggle = itemGo.AddComponent<Toggle>();

            var itemBg = new GameObject("Item Background");
            itemBg.layer = LayerMask.NameToLayer("UI");
            itemBg.transform.SetParent(itemGo.transform, false);
            var itemBgRt = itemBg.AddComponent<RectTransform>();
            itemBgRt.anchorMin = Vector2.zero;
            itemBgRt.anchorMax = Vector2.one;
            itemBgRt.sizeDelta = Vector2.zero;
            var img = itemBg.AddComponent<Image>();
            img.color = Color.clear;

            var itemTxtGo = new GameObject("Item Label");
            itemTxtGo.layer = LayerMask.NameToLayer("UI");
            itemTxtGo.transform.SetParent(itemGo.transform, false);
            var itemTxtRt = itemTxtGo.AddComponent<RectTransform>();
            itemTxtRt.anchorMin = Vector2.zero;
            itemTxtRt.anchorMax = Vector2.one;
            itemTxtRt.offsetMin = new Vector2(20f, 0f);
            itemTxtRt.offsetMax = new Vector2(-20f, 0f);
            var itemTxt = itemTxtGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                itemTxt.font = fontAsset;
            }

            itemTxt.fontSize = ControlValueFontSize;
            itemTxt.alignment = TextAlignmentOptions.Center;
            itemTxt.color = new Color(0.92f, 0.88f, 0.8f, 1f); // 羊皮纸暖白/米色

            itemToggle.targetGraphic = img;
            itemToggle.transition = Selectable.Transition.ColorTint;
            var cb = itemToggle.colors;
            cb.normalColor = Color.clear;
            cb.highlightedColor = new Color(0.95f, 0.85f, 0.6f, 0.2f); // 悬停时淡金色高亮
            cb.pressedColor = new Color(0.95f, 0.85f, 0.6f, 0.3f);
            cb.selectedColor = new Color(0.95f, 0.85f, 0.6f, 0.15f);
            cb.disabledColor = Color.clear;
            itemToggle.colors = cb;
            dropdown.itemText = itemTxt;
            dropdown.template = templateRt;

            UIEditorCreatorUtility.SetPrivateField(arrowRotator, "arrowRectTransform", arrowRt);
            UIEditorCreatorUtility.SetPrivateField(arrowRotator, "dropdown", dropdown);
            UIEditorCreatorUtility.SetPrivateField(arrowRotator, "templateRt", templateRt);
            UIEditorCreatorUtility.SetPrivateField(arrowRotator, "viewportRt", viewportRt);
            UIEditorCreatorUtility.SetPrivateField(arrowRotator, "contentRt", contentRt);

            // 序列化关联组件
            var existingSettings = LoadExistingSettings();
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "labelText", labelTxt);
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "dropdown", dropdown);
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "clickSoundId", existingSettings.ClickSoundId);
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "selectSoundId", existingSettings.SelectSoundId);
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "sparkleTemplate", sparkleTemplateGo);
            UIEditorCreatorUtility.SetPrivateField(dropdownControl, "sparkleParticleCount", existingSettings.SparkleParticleCount);

            // 保存 Prefab
            var prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Successfully created DropdownControl prefab at {PrefabPath}");
        }

        private static DropdownControlSettings LoadExistingSettings()
        {
            const int defaultClickSoundId = 100012;
            const int defaultSelectSoundId = 100013;
            const int defaultSparkleParticleCount = 28;

            var settings = new DropdownControlSettings
            {
                ClickSoundId = defaultClickSoundId,
                SelectSoundId = defaultSelectSoundId,
                SparkleParticleCount = defaultSparkleParticleCount
            };

            var existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (existingPrefab == null)
            {
                return settings;
            }

            var existingControl = existingPrefab.GetComponent<DropdownControl>();
            if (existingControl == null)
            {
                return settings;
            }

            var clickId = UIEditorCreatorUtility.GetPrivateField(existingControl, "clickSoundId", defaultClickSoundId);
            settings.ClickSoundId = clickId <= 10000 ? defaultClickSoundId : clickId;

            var selectId = UIEditorCreatorUtility.GetPrivateField(existingControl, "selectSoundId", defaultSelectSoundId);
            settings.SelectSoundId = selectId <= 10000 ? defaultSelectSoundId : selectId;

            settings.SparkleParticleCount = Mathf.Max(0, UIEditorCreatorUtility.GetPrivateField(existingControl, "sparkleParticleCount", defaultSparkleParticleCount));
            return settings;
        }

        private struct DropdownControlSettings
        {
            public int ClickSoundId;
            public int SelectSoundId;
            public int SparkleParticleCount;
        }
    }
}