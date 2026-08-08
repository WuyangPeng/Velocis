using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    /// <summary>
    /// 生成通用输入框预制体（Assets/Game/UI/UIForms/Common/InputField.prefab）。
    /// 该预制体包含背景图、Viewport 遮罩、输入文本、占位提示文本，
    /// 可在任何 UI 表单中复用。
    /// </summary>
    public static class InputFieldCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/InputField.prefab";

        private const string InputFieldBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_bg.png";
        private const string FontPath               = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        // 1920×1080 设计坐标系下的默认尺寸
        private static readonly Vector2 DefaultSize = new Vector2(670f, 100f);
        private static readonly Vector2 ViewportOffset = new Vector2(40f, 0f);
        private static readonly Vector2 ViewportPadding = new Vector2(-59f, -22f);
        private const float TextFontSize = 32f;
        private const float PlaceholderFontSize = 27f;
        private static readonly Color TextColor = new Color(1f, 0.97f, 0.88f, 1f);
        private static readonly Color PlaceholderColor = new Color(0.68f, 0.78f, 0.9f, 0.95f);
        private static readonly Color CaretColor = new Color(1f, 0.92f, 0.55f, 1f);

        // ──────────────────────────────────────────────
        // 编辑器菜单入口
        // ──────────────────────────────────────────────

        [MenuItem("Generator/UI/Component/Create InputField Prefab")]
        public static void CreateInputFieldPrefab()
        {
            // 确保目标目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite        bgSprite  = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldBgSpritePath);

            GameObject rootGo = BuildInputField(fontAsset, bgSprite, "InputField");

            // 保存为 Prefab
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);

            if (prefabAsset != null)
                EditorUtility.SetDirty(prefabAsset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"InputField 预制体已生成：{PrefabPath}");
        }

        // ──────────────────────────────────────────────
        // 公共构建方法（供其他 Creator 直接调用）
        // ──────────────────────────────────────────────

        /// <summary>
        /// 构建一个输入框 GameObject，不保存为 Prefab。
        /// 适用于其他 Creator 无法找到预制体时的 fallback 构建。
        /// </summary>
        public static GameObject BuildInputField(TMP_FontAsset fontAsset, Sprite bgSprite, string goName = "InputField")
        {
            GameObject go = new GameObject(goName);
            go.layer = LayerMask.NameToLayer("UI");

            RectTransform goRt = go.AddComponent<RectTransform>();
            goRt.sizeDelta = DefaultSize;

            // 背景图
            Image bg = go.AddComponent<Image>();
            if (bgSprite != null)
            {
                bg.sprite = bgSprite;
                bg.type   = Image.Type.Sliced;
                bg.color  = Color.white;
            }
            else
            {
                bg.color = new Color(0.92f, 0.86f, 0.72f, 0.95f); // 备用浅米黄色
            }

            TMP_InputField inputField = go.AddComponent<TMP_InputField>();
            inputField.customCaretColor = true;
            inputField.caretColor = CaretColor;

            // Viewport（带 RectMask2D 遮罩）
            GameObject viewportGo = new GameObject("Viewport");
            viewportGo.layer = LayerMask.NameToLayer("UI");
            viewportGo.transform.SetParent(go.transform, false);
            RectTransform viewportRt = viewportGo.AddComponent<RectTransform>();
            viewportRt.anchorMin        = Vector2.zero;
            viewportRt.anchorMax        = Vector2.one;
            viewportRt.anchoredPosition = ViewportOffset;
            viewportRt.sizeDelta        = ViewportPadding;
            viewportGo.AddComponent<RectMask2D>();

            // 输入文本
            GameObject textGo = new GameObject("Text");
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(viewportGo.transform, false);
            TextMeshProUGUI textComp = textGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) textComp.font = fontAsset;
            textComp.fontSize  = TextFontSize;
            textComp.color     = TextColor;
            textComp.alignment = TextAlignmentOptions.Left;
            RectTransform textRt = textComp.GetComponent<RectTransform>();
            textRt.anchorMin        = Vector2.zero;
            textRt.anchorMax        = Vector2.one;
            textRt.anchoredPosition = Vector2.zero;
            textRt.sizeDelta        = Vector2.zero;

            // 占位提示文本
            GameObject placeholderGo = new GameObject("Placeholder");
            placeholderGo.layer = LayerMask.NameToLayer("UI");
            placeholderGo.transform.SetParent(viewportGo.transform, false);
            TextMeshProUGUI placeholderComp = placeholderGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) placeholderComp.font = fontAsset;
            placeholderComp.text      = string.Empty; // 占位文本由使用方赋值
            placeholderComp.fontSize  = PlaceholderFontSize;
            placeholderComp.color     = PlaceholderColor;
            placeholderComp.alignment = TextAlignmentOptions.Left;
            RectTransform placeholderRt = placeholderComp.GetComponent<RectTransform>();
            placeholderRt.anchorMin        = Vector2.zero;
            placeholderRt.anchorMax        = Vector2.one;
            placeholderRt.anchoredPosition = Vector2.zero;
            placeholderRt.sizeDelta        = Vector2.zero;

            // 绑定 TMP_InputField 子引用
            inputField.textViewport  = viewportRt;
            inputField.textComponent = textComp;
            inputField.placeholder   = placeholderComp;

            var inputFieldSound = go.AddComponent<BaseInputField>();
            SetPrivateField(inputFieldSound, "clickSound", 100001);
            SetPrivateField(inputFieldSound, "typeSound", 100002);

            return go;
        }

        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(
                fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (field != null)
                field.SetValue(obj, value);
            else
                Debug.LogWarning($"InputFieldCreator: 字段 '{fieldName}' 在 {obj.GetType().Name} 上未找到！");
        }
    }
}
