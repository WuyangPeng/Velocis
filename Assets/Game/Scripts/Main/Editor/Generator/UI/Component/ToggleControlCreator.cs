using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class ToggleControlCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/ToggleControl.prefab";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        private const float ControlGroupWidth = 620f;
        private const float ControlLabelFontSize = 22f;

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

        [MenuItem("Generator/UI/Component/Create Toggle Control Prefab")]
        public static void CreateToggleControlPrefab()
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

            ConfigureTextureAsSprite("Assets/Game/Textures/Toggle/Menu/toggle_off.png");
            ConfigureTextureAsSprite("Assets/Game/Textures/Toggle/Menu/toggle_on.png");

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite toggleOffSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Toggle/Menu/toggle_off.png");
            Sprite toggleOnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Toggle/Menu/toggle_on.png");

            GameObject rootGo = new GameObject("ToggleControlGroup");
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(ControlGroupWidth, 48f);

            ToggleControl toggleControl = rootGo.AddComponent<ToggleControl>();

            // 1. Toggle 组件
            GameObject toggleGo = new GameObject("Toggle");
            toggleGo.layer = LayerMask.NameToLayer("UI");
            toggleGo.transform.SetParent(rootGo.transform, false);
            RectTransform toggleRt = toggleGo.AddComponent<RectTransform>();
            toggleRt.anchorMin = new Vector2(0f, 0.5f);
            toggleRt.anchorMax = new Vector2(0f, 0.5f);
            toggleRt.pivot = new Vector2(0f, 0.5f);
            toggleRt.anchoredPosition = new Vector2(10f, 0f);
            toggleRt.sizeDelta = new Vector2(36f, 36f);
            Toggle toggle = toggleGo.AddComponent<Toggle>();

            GameObject bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(toggleGo.transform, false);
            RectTransform bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(36f, 36f);
            Image bgImg = bgGo.AddComponent<Image>();
            if (toggleOffSprite != null)
            {
                bgImg.sprite = toggleOffSprite;
                bgImg.color = Color.white;
            }
            else
            {
                bgImg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            }

            GameObject checkGo = new GameObject("Checkmark");
            checkGo.layer = LayerMask.NameToLayer("UI");
            checkGo.transform.SetParent(toggleGo.transform, false);
            RectTransform checkRt = checkGo.AddComponent<RectTransform>();
            checkRt.sizeDelta = new Vector2(34f, 34f);
            Image checkImg = checkGo.AddComponent<Image>();
            if (toggleOnSprite != null)
            {
                checkImg.sprite = toggleOnSprite;
                checkImg.color = Color.white;
            }
            else
            {
                checkImg.color = new Color(0.95f, 0.85f, 0.6f, 1f);
            }

            toggle.targetGraphic = bgImg;
            toggle.graphic = checkImg;
            toggle.isOn = true;

            // 2. 文本标签
            GameObject textGo = new GameObject("Label");
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(rootGo.transform, false);
            RectTransform textRt = textGo.AddComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0f, 0.5f);
            textRt.anchorMax = new Vector2(1f, 0.5f);
            textRt.pivot = new Vector2(0f, 0.5f);
            textRt.anchoredPosition = new Vector2(58f, 0f);
            textRt.sizeDelta = new Vector2(-70f, 36f);
            TextMeshProUGUI txt = textGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                txt.font = fontAsset;
            }
            txt.text = "ToggleLabel";
            txt.fontSize = ControlLabelFontSize;
            txt.alignment = TextAlignmentOptions.Left;
            txt.color = Color.white;

            // 3. 关联绑定私有字段
            UIEditorCreatorUtility.SetPrivateField(toggleControl, "toggle", toggle);
            UIEditorCreatorUtility.SetPrivateField(toggleControl, "labelText", txt);

            // 保存为 Prefab
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            if (prefabAsset != null)
            {
                EditorUtility.SetDirty(prefabAsset);
            }

            Object.DestroyImmediate(rootGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"ToggleControl 预制体已生成：{PrefabPath}");
        }
    }
}
