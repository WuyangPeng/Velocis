using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class SliderCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/Slider.prefab";

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

        [MenuItem("Generator/UI/Component/Create Slider Prefab")]
        public static void CreateSliderPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Common/Control";
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
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
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "Control");
            }

            ConfigureTextureAsSprite("Assets/Game/Textures/Slider/Menu/slider_track.png", new Vector4(30f, 0f, 30f, 0f));
            ConfigureTextureAsSprite("Assets/Game/Textures/Slider/Menu/slider_handle.png");

            Sprite trackSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Slider/Menu/slider_track.png");
            Sprite handleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Slider/Menu/slider_handle.png");

            GameObject sliderGo = new GameObject("Slider");
            sliderGo.layer = LayerMask.NameToLayer("UI");
            RectTransform sliderRt = sliderGo.AddComponent<RectTransform>();
            sliderRt.sizeDelta = new Vector2(390f, 24f);
            Slider slider = sliderGo.AddComponent<Slider>();

            // 滑块背景 (Track)
            GameObject trackGo = new GameObject("Background");
            trackGo.layer = LayerMask.NameToLayer("UI");
            trackGo.transform.SetParent(sliderGo.transform, false);
            RectTransform trackRt = trackGo.AddComponent<RectTransform>();
            trackRt.anchorMin = new Vector2(0f, 0f);
            trackRt.anchorMax = new Vector2(1f, 1f);
            trackRt.anchoredPosition = Vector2.zero;
            trackRt.sizeDelta = Vector2.zero;
            Image trackImg = trackGo.AddComponent<Image>();
            if (trackSprite != null)
            {
                trackImg.sprite = trackSprite;
                trackImg.type = Image.Type.Sliced;
                trackImg.color = Color.white;
            }
            else
            {
                trackImg.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
            }

            // 填充区域 (Fill Area)
            GameObject fillAreaGo = new GameObject("Fill Area");
            fillAreaGo.layer = LayerMask.NameToLayer("UI");
            fillAreaGo.transform.SetParent(sliderGo.transform, false);
            RectTransform fillAreaRt = fillAreaGo.AddComponent<RectTransform>();
            fillAreaRt.anchorMin = new Vector2(0f, 0.35f);
            fillAreaRt.anchorMax = new Vector2(1f, 0.65f);
            fillAreaRt.anchoredPosition = Vector2.zero;
            fillAreaRt.sizeDelta = new Vector2(-20f, 0f);

            GameObject fillGo = new GameObject("Fill");
            fillGo.layer = LayerMask.NameToLayer("UI");
            fillGo.transform.SetParent(fillAreaGo.transform, false);
            RectTransform fillRt = fillGo.AddComponent<RectTransform>();
            fillRt.sizeDelta = Vector2.zero;
            Image fillImg = fillGo.AddComponent<Image>();
            fillImg.color = new Color(0.82f, 0.66f, 0.32f, 0.85f); // Warm glowing gold/bronze instead of bright solid yellow

            // 滑块手柄 (Handle Slide Area)
            GameObject handleAreaGo = new GameObject("Handle Slide Area");
            handleAreaGo.layer = LayerMask.NameToLayer("UI");
            handleAreaGo.transform.SetParent(sliderGo.transform, false);
            RectTransform handleAreaRt = handleAreaGo.AddComponent<RectTransform>();
            handleAreaRt.anchorMin = Vector2.zero;
            handleAreaRt.anchorMax = Vector2.one;
            handleAreaRt.sizeDelta = Vector2.zero;

            GameObject handleGo = new GameObject("Handle");
            handleGo.layer = LayerMask.NameToLayer("UI");
            handleGo.transform.SetParent(handleAreaGo.transform, false);
            RectTransform handleRt = handleGo.AddComponent<RectTransform>();
            handleRt.sizeDelta = new Vector2(24f, 0f); // y = 0 prevents stretching when driven by the Slider anchor
            Image handleImg = handleGo.AddComponent<Image>();
            if (handleSprite != null)
            {
                handleImg.sprite = handleSprite;
                handleImg.color = Color.white;
            }
            else
            {
                handleImg.color = Color.white;
            }

            slider.fillRect = fillRt;
            slider.handleRect = handleRt;
            slider.targetGraphic = handleImg;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 1f;

            // Create hover particles layer and sparkle template under sliderGo
            UIEditorCreatorUtility.CreateHoverParticleLayer(sliderGo.transform, new Vector2(20f, 10f));
            GameObject sparkleTemplateGo = UIEditorCreatorUtility.CreateSparkleTemplate(sliderGo.transform, new Vector2(48f, 48f));

            // Add SliderControl component and bind fields
            var sliderControl = sliderGo.AddComponent<SliderControl>();
            UIEditorCreatorUtility.SetPrivateField(sliderControl, "slider", slider);
            UIEditorCreatorUtility.SetPrivateField(sliderControl, "dragSoundId", 100010);
            UIEditorCreatorUtility.SetPrivateField(sliderControl, "sparkleTemplate", sparkleTemplateGo);

            // 保存为 Prefab
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(sliderGo, PrefabPath);
            if (prefabAsset != null)
            {
                EditorUtility.SetDirty(prefabAsset);
            }

            Object.DestroyImmediate(sliderGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"Successfully created Slider prefab at {PrefabPath}");
        }
    }
}
