using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class VolumeControlCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/VolumeControl.prefab";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        private const float ControlGroupWidth = 620f;
        private const float ControlLabelWidth = 120f;
        private const float ControlLabelFontSize = 22f;

        private static void ConfigureTextureAsSprite(string path, Vector4 border = default)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                bool changed = false;
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

        [MenuItem("Generator/UI/Component/Create Volume Control Prefab")]
        public static void CreateVolumeControlPrefab()
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

            GameObject rootGo = new GameObject("VolumeControlGroup");
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(ControlGroupWidth, 48f);

            // 标签文本
            GameObject labelGo = new GameObject("Label");
            labelGo.layer = LayerMask.NameToLayer("UI");
            labelGo.transform.SetParent(rootGo.transform, false);
            RectTransform labelRt = labelGo.AddComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 0.5f);
            labelRt.anchorMax = new Vector2(0f, 0.5f);
            labelRt.pivot = new Vector2(0f, 0.5f);
            labelRt.anchoredPosition = new Vector2(10f, 0f);
            labelRt.sizeDelta = new Vector2(ControlLabelWidth, 36f);
            TextMeshProUGUI labelTxt = labelGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) labelTxt.font = fontAsset;
            labelTxt.text = "VolumeLabel";
            labelTxt.fontSize = ControlLabelFontSize;
            labelTxt.alignment = TextAlignmentOptions.Left;
            labelTxt.color = Color.white;

            // 静音开关 Toggle
            GameObject muteGo = new GameObject("MuteToggle");
            muteGo.layer = LayerMask.NameToLayer("UI");
            muteGo.transform.SetParent(rootGo.transform, false);
            RectTransform muteRt = muteGo.AddComponent<RectTransform>();
            muteRt.anchorMin = new Vector2(0f, 0.5f);
            muteRt.anchorMax = new Vector2(0f, 0.5f);
            muteRt.pivot = new Vector2(0f, 0.5f);
            muteRt.anchoredPosition = new Vector2(145f, 0f);
            muteRt.sizeDelta = new Vector2(36f, 36f);
            Toggle toggle = muteGo.AddComponent<Toggle>();

            GameObject bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(muteGo.transform, false);
            RectTransform bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(36f, 36f); // Matches Toggle parent bounds
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
            checkGo.transform.SetParent(muteGo.transform, false);
            RectTransform checkRt = checkGo.AddComponent<RectTransform>();
            checkRt.sizeDelta = new Vector2(34f, 34f); // Sized up to make the tick look large and fill the frame
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

            // 加载/创建滑条 Slider 预制体
            GameObject sliderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SliderCreator.PrefabPath);
            if (sliderPrefab == null)
            {
                SliderCreator.CreateSliderPrefab();
                sliderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SliderCreator.PrefabPath);
            }

            GameObject sliderGo = PrefabUtility.InstantiatePrefab(sliderPrefab, rootGo.transform) as GameObject;
            sliderGo.name = "Slider";
            RectTransform sliderRt = sliderGo.GetComponent<RectTransform>();
            sliderRt.anchorMin = new Vector2(0f, 0.5f);
            sliderRt.anchorMax = new Vector2(1f, 0.5f);
            sliderRt.pivot = new Vector2(0f, 0.5f);
            sliderRt.anchoredPosition = new Vector2(200f, 0f);
            sliderRt.sizeDelta = new Vector2(-230f, 24f);

            Slider slider = sliderGo.GetComponent<Slider>();
            SliderControl sliderControl = sliderGo.GetComponent<SliderControl>();

            // Add VolumeControl component and bind fields
            var volumeControl = rootGo.AddComponent<Game.Scripts.Hotfix.HotfixBusiness.UI.Common.VolumeControl>();
            UIEditorCreatorUtility.SetPrivateField(volumeControl, "labelText", labelTxt);
            UIEditorCreatorUtility.SetPrivateField(volumeControl, "muteToggle", toggle);
            UIEditorCreatorUtility.SetPrivateField(volumeControl, "volumeSlider", sliderControl);
            UIEditorCreatorUtility.SetPrivateField(volumeControl, "switchSoundId", 100011);

            UnityEventTools.AddPersistentListener(toggle.onValueChanged, volumeControl.PlaySwitchSound);
            UnityEventTools.AddPersistentListener(slider.onValueChanged, sliderControl.PlayDragSound);

            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Successfully created VolumeControl prefab at {PrefabPath}");
        }
    }
}
