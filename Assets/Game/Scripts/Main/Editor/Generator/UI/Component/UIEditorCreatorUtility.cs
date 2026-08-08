using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class UIEditorCreatorUtility
    {
        public const string SparkleTemplateNodeName = "GoldSparkleTemplate";
        public const string SparkleSpritePath = "Assets/Game/Textures/Effects/Button/Menu/rectangle_gold_sparkle.png";
        
        public static readonly Vector2 DefaultHoverParticleExpand = new Vector2(110f, 58f);
        public static readonly Vector2 SparkleTemplateSize = new Vector2(32f, 32f);

        public static void CreateHoverParticleLayer(Transform parent, Vector2 expand)
        {
            GameObject particlesGo = new GameObject("HoverParticles");
            particlesGo.layer = LayerMask.NameToLayer("UI");
            particlesGo.transform.SetParent(parent, false);

            RectTransform particlesRect = particlesGo.AddComponent<RectTransform>();
            particlesRect.anchorMin = Vector2.zero;
            particlesRect.anchorMax = Vector2.one;
            particlesRect.anchoredPosition = Vector2.zero;
            particlesRect.sizeDelta = expand;
            particlesRect.pivot = new Vector2(0.5f, 0.5f);

            Canvas canvas = particlesGo.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 10;

            particlesGo.SetActive(false);
        }

        public static GameObject CreateSparkleTemplate(Transform parent, Vector2 templateSize)
        {
            Sprite sparkleSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SparkleSpritePath);
            if (sparkleSprite == null)
            {
                Debug.LogWarning($"UIEditorCreatorUtility: sparkle sprite not found at {SparkleSpritePath}!");
            }

            GameObject templateGo = new GameObject(SparkleTemplateNodeName);
            templateGo.layer = LayerMask.NameToLayer("UI");
            templateGo.transform.SetParent(parent, false);

            Image sparkle = templateGo.AddComponent<Image>();
            sparkle.sprite = sparkleSprite;
            sparkle.raycastTarget = false;
            sparkle.preserveAspect = true;
            sparkle.color = new Color(1f, 0.82f, 0.18f, 0f);

            RectTransform rect = templateGo.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = templateSize;
            rect.localScale = Vector3.zero;
            templateGo.SetActive(false);
            return templateGo;
        }

        public static void SetPrivateField(object obj, string fieldName, object value)
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

            Debug.LogWarning($"UIEditorCreatorUtility: 字段 '{fieldName}' 在 {obj.GetType().Name} 及其基类上未找到！");
        }

        public static T GetPrivateField<T>(object obj, string fieldName, T fallback)
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

                if (field.GetValue(obj) is T value)
                {
                    return value;
                }

                return fallback;
            }

            Debug.LogWarning($"UIEditorCreatorUtility: 字段 '{fieldName}' 在 {obj.GetType().Name} 及其基类上未找到！");
            return fallback;
        }
    }
}
