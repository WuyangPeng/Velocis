// 创建时间：2026-07-26
// 修改时间：2026-07-26

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    /// <summary>
    /// 生成带 Label 的输入框组合预制体（Assets/Game/UI/UIForms/Common/Control/LabeledInputFieldControl.prefab）。
    /// </summary>
    public static class LabeledInputFieldControlCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/LabeledInputFieldControl.prefab";
        private const string InputFieldShortBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_short_bg.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        private const float ControlGroupWidth = 320f;
        private const float ControlGroupHeight = 52f;

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

        [MenuItem("Generator/UI/Component/Create Labeled InputField Control Prefab")]
        public static void CreateLabeledInputFieldControlPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/Common/Control";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
                {
                    AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
                }
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "Control");
            }

            ConfigureTextureAsSprite(InputFieldShortBgSpritePath);

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var inputShortBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldShortBgSpritePath);
            var inputFieldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(InputFieldCreator.PrefabPath);

            var rootGo = new GameObject("LabeledInputFieldControlGroup");
            rootGo.layer = LayerMask.NameToLayer("UI");
            var rt = rootGo.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(ControlGroupWidth, ControlGroupHeight);

            var controlComp = rootGo.AddComponent<LabeledInputFieldControl>();

            // 1. Label 文本标签
            var labelGo = new GameObject("Label");
            labelGo.layer = LayerMask.NameToLayer("UI");
            labelGo.transform.SetParent(rootGo.transform, false);
            var labelRt = labelGo.AddComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 0.5f);
            labelRt.anchorMax = new Vector2(0f, 0.5f);
            labelRt.pivot = new Vector2(0f, 0.5f);
            labelRt.anchoredPosition = Vector2.zero;
            labelRt.sizeDelta = new Vector2(100f, 48f);

            var labelText = labelGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                labelText.font = fontAsset;
            }
            labelText.text = "LabelText";
            labelText.fontSize = 22f;
            labelText.enableWordWrapping = false;
            labelText.alignment = TextAlignmentOptions.Left;
            labelText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // 2. InputField 输入框组件
            GameObject inputGo;
            if (inputFieldPrefab != null)
            {
                inputGo = (GameObject)PrefabUtility.InstantiatePrefab(inputFieldPrefab);
                inputGo.name = "InputField";
            }
            else
            {
                inputGo = InputFieldCreator.BuildInputField(fontAsset, inputShortBgSprite, "InputField");
            }

            inputGo.transform.SetParent(rootGo.transform, false);
            var inputRt = inputGo.GetComponent<RectTransform>();
            inputRt.anchorMin = new Vector2(0f, 0.5f);
            inputRt.anchorMax = new Vector2(0f, 0.5f);
            inputRt.pivot = new Vector2(0f, 0.5f);
            inputRt.anchoredPosition = new Vector2(105f, 0f);
            inputRt.sizeDelta = new Vector2(210f, 52f);

            if (inputShortBgSprite != null && inputGo.GetComponent<Image>() is Image inputImg)
            {
                inputImg.sprite = inputShortBgSprite;
            }

            var inputField = inputGo.GetComponent<TMP_InputField>();
            if (inputField != null)
            {
                inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                if (inputField.textViewport != null)
                {
                    inputField.textViewport.anchoredPosition = Vector2.zero;
                    inputField.textViewport.sizeDelta = new Vector2(-28f, -11f);
                }
                if (inputField.textComponent is TMP_Text textComp)
                {
                    textComp.enableWordWrapping = false;
                    textComp.color = new Color(0.05f, 0.2f, 0.25f, 1f);
                }
                if (inputField.placeholder is TMP_Text placeholderComp)
                {
                    placeholderComp.enableWordWrapping = false;
                    placeholderComp.color = new Color(0.15f, 0.35f, 0.4f, 0.85f);
                }
            }

            // 3. 关联绑定私有字段
            UIEditorCreatorUtility.SetPrivateField(controlComp, "labelText", labelText);
            UIEditorCreatorUtility.SetPrivateField(controlComp, "inputField", inputField);

            // 保存 Prefab
            var prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            if (prefabAsset != null)
            {
                EditorUtility.SetDirty(prefabAsset);
            }

            Object.DestroyImmediate(rootGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"LabeledInputFieldControl 预制体已生成：{PrefabPath}");
        }
    }
}
