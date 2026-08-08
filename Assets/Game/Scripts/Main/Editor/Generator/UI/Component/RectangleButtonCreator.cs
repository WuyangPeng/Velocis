using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class RectangleButtonCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string RectangleSpritePath = "Assets/Game/Textures/Button/Menu/rectangle_default.png";

        // 1920×1080 设计坐标系下的默认尺寸
        private static readonly Vector2 DefaultSize = new Vector2(250f, 70f);
        private const float DefaultFontSize = 32f;
        [MenuItem("Generator/UI/Component/Create Rectangle Button Prefab")]
        public static void CreateRectangleButtonPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            var existingSettings = LoadExistingButtonSettings();

            GameObject go = new GameObject("RectangleButton");
            go.layer = LayerMask.NameToLayer("UI");

            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = DefaultSize;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            go.AddComponent<RectangleButton>();
            var rectangleButton = go.GetComponent<RectangleButton>();
            UIEditorCreatorUtility.SetPrivateField(rectangleButton, "clickSoundId", existingSettings.ClickSoundId);
            UIEditorCreatorUtility.SetPrivateField(rectangleButton, "hoverSoundId", existingSettings.HoverSoundId);
            UIEditorCreatorUtility.SetPrivateField(rectangleButton, "sparkleParticleCount", existingSettings.SparkleParticleCount);

            GameObject imageGo = new GameObject("Image");
            imageGo.layer = LayerMask.NameToLayer("UI");
            imageGo.transform.SetParent(go.transform);

            Image image = imageGo.AddComponent<Image>();
            image.raycastTarget = true;

            Sprite rectSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RectangleSpritePath);
            if (rectSprite != null)
            {
                image.sprite = rectSprite;
                image.type = Image.Type.Sliced;
            }
            else
            {
                Debug.LogWarning($"RectangleButtonCreator: rectangle_default.png not found at {RectangleSpritePath}!");
            }

            RectTransform imageRect = image.GetComponent<RectTransform>();
            imageRect.anchorMin = Vector2.zero;
            imageRect.anchorMax = Vector2.one;
            imageRect.anchoredPosition = Vector2.zero;
            imageRect.sizeDelta = Vector2.zero;
            imageRect.pivot = new Vector2(0.5f, 0.5f);

            GameObject textGo = new GameObject("Text");
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(go.transform);

            TextMeshProUGUI text = textGo.AddComponent<TextMeshProUGUI>();

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Game/Fonts/NotoSerifSC-Black SDF.asset");
            if (fontAsset != null)
            {
                text.font = fontAsset;
            }
            else
            {
                Debug.LogWarning("RectangleButtonCreator: NotoSerifSC-Black SDF.asset not found under Assets/Game/Fonts/!");
            }

            text.text = "确认";
            text.fontSize = DefaultFontSize;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            text.raycastTarget = false;
            // Text does not need special vertex sorting to render behind siblings (handled via child Canvas override sorting on the particle container)

            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = Vector2.zero;

            GameObject sparkleTemplate = UIEditorCreatorUtility.CreateSparkleTemplate(go.transform, UIEditorCreatorUtility.SparkleTemplateSize);
            UIEditorCreatorUtility.SetPrivateField(rectangleButton, "sparkleTemplate", sparkleTemplate);

            UIEditorCreatorUtility.CreateHoverParticleLayer(go.transform, UIEditorCreatorUtility.DefaultHoverParticleExpand);

            PrefabUtility.SaveAsPrefabAsset(go, PrefabPath);
            Object.DestroyImmediate(go);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Successfully created RectangleButton prefab at {PrefabPath}.");
        }

        private static RectangleButtonSettings LoadExistingButtonSettings()
        {
            const int defaultClickSoundId = 100004;
            const int defaultHoverSoundId = 100006;
            const int defaultSparkleParticleCount = 28;

            var settings = new RectangleButtonSettings
            {
                ClickSoundId = defaultClickSoundId,
                HoverSoundId = defaultHoverSoundId,
                SparkleParticleCount = defaultSparkleParticleCount
            };

            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (existingPrefab == null)
            {
                return settings;
            }

            RectangleButton existingButton = existingPrefab.GetComponent<RectangleButton>();
            if (existingButton == null)
            {
                return settings;
            }

            settings.ClickSoundId = UIEditorCreatorUtility.GetPrivateField(existingButton, "clickSoundId", defaultClickSoundId);
            settings.HoverSoundId = UIEditorCreatorUtility.GetPrivateField(existingButton, "hoverSoundId", defaultHoverSoundId);
            settings.SparkleParticleCount = Mathf.Max(0, UIEditorCreatorUtility.GetPrivateField(existingButton, "sparkleParticleCount", defaultSparkleParticleCount));
            return settings;
        }

        private struct RectangleButtonSettings
        {
            public int ClickSoundId;
            public int HoverSoundId;
            public int SparkleParticleCount;
        }

    }
}
