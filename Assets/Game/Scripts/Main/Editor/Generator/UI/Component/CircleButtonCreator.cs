using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class CircleButtonCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/Common/Button/CircleButton.prefab";
        private const string CircleSpritePath = "Assets/Game/Textures/Button/Menu/circle_default.png";
        private const string GlowSpritePath = "Assets/Game/Textures/Effects/Button/Menu/circle_button_hover_glow.png";

        // 1920×1080 设计坐标系下的默认尺寸
        private static readonly Vector2 DefaultSize = new Vector2(100f, 100f);
        private static readonly Vector2 GlowExpandSize = new Vector2(34f, 34f);
        private static readonly Vector2 TextAreaSize = new Vector2(140f, 42f);
        private const float DefaultFontSize = 28f;

        [MenuItem("Generator/UI/Component/Create Circle Button Prefab")]
        public static void CreateCircleButtonPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            GameObject go = new GameObject("CircleButton");
            go.layer = LayerMask.NameToLayer("UI");

            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = DefaultSize;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            go.AddComponent<CircleButton>();
            var circleButton = go.GetComponent<CircleButton>();
            SetPrivateField(circleButton, "clickSoundId", 100003);
            SetPrivateField(circleButton, "hoverSoundId", 100005);

            var glowImg = CreateGlowLayer(go.transform, GlowSpritePath, new Color(1f, 0.82f, 0.35f, 0f), GlowExpandSize);
            SetPrivateField(circleButton, "glowImage", glowImg);

            // 添加与 Text 同级的 Image 子节点
            GameObject imageGo = new GameObject("Image");
            imageGo.layer = LayerMask.NameToLayer("UI");
            imageGo.transform.SetParent(go.transform);

            Image image = imageGo.AddComponent<Image>();
            image.raycastTarget = true;

            Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CircleSpritePath);
            if (circleSprite != null)
            {
                image.sprite = circleSprite;
            }
            else
            {
                Debug.LogWarning($"CircleButtonCreator: circle_default.png not found at {CircleSpritePath}!");
            }

            RectTransform imageRect = image.GetComponent<RectTransform>();
            imageRect.anchorMin = Vector2.zero;
            imageRect.anchorMax = Vector2.one;
            imageRect.anchoredPosition = Vector2.zero;
            imageRect.sizeDelta = Vector2.zero;
            imageRect.pivot = new Vector2(0.5f, 0.5f);

            // 在按钮下方添加文本
            GameObject textGo = new GameObject("Text");
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(go.transform);

            TextMeshProUGUI text = textGo.AddComponent<TextMeshProUGUI>();
            
            // 从 Assets/Game/Fonts/ 加载并分配字体资源
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Game/Fonts/NotoSerifSC-Black SDF.asset");
            if (fontAsset != null)
            {
                text.font = fontAsset;
            }
            else
            {
                Debug.LogWarning("CircleButtonCreator: NotoSerifSC-Black SDF.asset not found under Assets/Game/Fonts/!");
            }

            text.text = "按钮";
            text.fontSize = DefaultFontSize;
            text.alignment = TextAlignmentOptions.Top;
            text.color = Color.white;

            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0f);
            textRect.anchorMax = new Vector2(0.5f, 0f);
            textRect.pivot = new Vector2(0.5f, 1f); // 轴心点设为顶部居中
            textRect.anchoredPosition = Vector2.zero; // 自然对齐，无需硬编码偏移
            textRect.sizeDelta = TextAreaSize;

            SetPrivateField(circleButton, "btnText", text);

            PrefabUtility.SaveAsPrefabAsset(go, PrefabPath);
            Object.DestroyImmediate(go);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Successfully created CircleButton prefab at {PrefabPath}");
        }

        private static Image CreateGlowLayer(Transform parent, string spritePath, Color glowColor, Vector2 expandSize)
        {
            GameObject glowGo = new GameObject("Glow");
            glowGo.layer = LayerMask.NameToLayer("UI");
            glowGo.transform.SetParent(parent, false);

            Image glow = glowGo.AddComponent<Image>();
            glow.raycastTarget = false;

            Sprite glowSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (glowSprite != null)
            {
                glow.sprite = glowSprite;
            }
            else
            {
                Debug.LogWarning($"CircleButtonCreator: glow sprite not found at {spritePath}!");
            }

            glow.color = glowColor;

            RectTransform glowRect = glow.GetComponent<RectTransform>();
            glowRect.anchorMin = Vector2.zero;
            glowRect.anchorMax = Vector2.one;
            glowRect.anchoredPosition = Vector2.zero;
            glowRect.sizeDelta = expandSize;
            glowRect.pivot = new Vector2(0.5f, 0.5f);

            return glow;
        }

        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            for (var type = obj.GetType(); type != null; type = type.BaseType)
            {
                var field = type.GetField(
                    fieldName,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);

                if (field == null)
                {
                    continue;
                }

                field.SetValue(obj, value);
                return;
            }

            Debug.LogWarning($"CircleButtonCreator: 字段 '{fieldName}' 在 {obj.GetType().Name} 及其基类上未找到！");
        }
    }
}
