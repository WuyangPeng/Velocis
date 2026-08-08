// 创建时间：2026-07-31
// 修改时间：2026-08-03
// 审核时间：

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Menu;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Main.Editor.Generator.UI.Component;
using Game.Scripts.Main.Runtime.UI.UICommon;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class LoginLoadFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/LoginLoadForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Background/Menu/login_loading_bg_01.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

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

        [MenuItem("Generator/UI/Form/Create Login Load Form Prefab")]
        public static void CreateLoginLoadFormPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/System";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "System");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);

            // Create root
            GameObject rootGo = CreateRoot();
            LoginLoadForm form = rootGo.AddComponent<LoginLoadForm>();
            CanvasGroup rootCg = rootGo.GetComponent<CanvasGroup>() ?? rootGo.AddComponent<CanvasGroup>();

            // 1. Fullscreen Background Image
            GameObject bgGo = new GameObject("Background", typeof(RectTransform));
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            RectTransform bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;
            Image bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Simple;
            }
            bgImg.color = Color.white;

            // 2. Slider (Progress Bar)
            GameObject sliderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SliderCreator.PrefabPath);
            if (sliderPrefab == null)
            {
                SliderCreator.CreateSliderPrefab();
                sliderPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(SliderCreator.PrefabPath);
            }

            GameObject sliderGo = PrefabUtility.InstantiatePrefab(sliderPrefab, rootGo.transform) as GameObject;
            sliderGo.name = "ProgressBar";
            RectTransform sliderRt = sliderGo.GetComponent<RectTransform>();
            sliderRt.anchorMin = new Vector2(0.5f, 0f);
            sliderRt.anchorMax = new Vector2(0.5f, 0f);
            sliderRt.pivot = new Vector2(0.5f, 0f);
            sliderRt.anchoredPosition = new Vector2(0f, 150f);
            sliderRt.sizeDelta = new Vector2(1000f, 24f);

            Slider slider = sliderGo.GetComponent<Slider>();
            slider.transition = Selectable.Transition.None;
            slider.interactable = false;

            SliderControl sliderControl = sliderGo.GetComponent<SliderControl>();

            // 3. Percentage Text
            GameObject percentGo = new GameObject("PercentageText", typeof(RectTransform));
            percentGo.layer = LayerMask.NameToLayer("UI");
            percentGo.transform.SetParent(rootGo.transform, false);
            RectTransform percentRt = percentGo.GetComponent<RectTransform>();
            percentRt.anchorMin = new Vector2(0.5f, 0f);
            percentRt.anchorMax = new Vector2(0.5f, 0f);
            percentRt.pivot = new Vector2(0.5f, 0f);
            percentRt.anchoredPosition = new Vector2(0f, 190f);
            percentRt.sizeDelta = new Vector2(600f, 40f);
            TextMeshProUGUI percentText = percentGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) percentText.font = fontAsset;
            percentText.text = "调兵遣将中... 0%";
            percentText.fontSize = 30f;
            percentText.alignment = TextAlignmentOptions.Center;
            percentText.color = Color.white; // 纯白色字 (White text)
            percentText.outlineColor = new Color32(0, 0, 0, 255); // 纯黑描边 (Pure black outline)
            percentText.outlineWidth = 0.25f; // 加粗描边宽度 (Thicker outline)

            // 4. Tips Text & Canvas Group (Tips Container)
            GameObject tipsContainerGo = new GameObject("TipsContainer", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
            tipsContainerGo.layer = LayerMask.NameToLayer("UI");
            tipsContainerGo.transform.SetParent(rootGo.transform, false);
            RectTransform tipsContainerRt = tipsContainerGo.GetComponent<RectTransform>();
            tipsContainerRt.anchorMin = new Vector2(0.5f, 0f);
            tipsContainerRt.anchorMax = new Vector2(0.5f, 0f);
            tipsContainerRt.pivot = new Vector2(0.5f, 0f);
            tipsContainerRt.anchoredPosition = new Vector2(0f, 60f);
            tipsContainerRt.sizeDelta = new Vector2(1000f, 60f);
            CanvasGroup tipsCg = tipsContainerGo.GetComponent<CanvasGroup>();
            Image tipsBg = tipsContainerGo.GetComponent<Image>();
            tipsBg.color = new Color(0f, 0f, 0f, 0.55f); // 半透明纯黑底衬 (Semi-transparent black background)

            GameObject tipsTextGo = new GameObject("TipsText", typeof(RectTransform));
            tipsTextGo.layer = LayerMask.NameToLayer("UI");
            tipsTextGo.transform.SetParent(tipsContainerGo.transform, false);
            RectTransform tipsTextRt = tipsTextGo.GetComponent<RectTransform>();
            tipsTextRt.anchorMin = Vector2.zero;
            tipsTextRt.anchorMax = Vector2.one;
            tipsTextRt.sizeDelta = Vector2.zero;
            TextMeshProUGUI tipsText = tipsTextGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) tipsText.font = fontAsset;
            tipsText.text = "正在研读兵书...";
            tipsText.fontSize = 30f;
            tipsText.alignment = TextAlignmentOptions.Center;
            tipsText.color = Color.white; // 纯白色字 (White text)
            tipsText.outlineColor = new Color32(0, 0, 0, 255); // 纯黑描边 (Pure black outline)
            tipsText.outlineWidth = 0.25f; // 加粗描边宽度 (Thicker outline)

            // Bind fields via reflection
            SetPrivateField(form, "bgImage", bgImg);
            SetPrivateField(form, "progressSlider", sliderControl);
            SetPrivateField(form, "percentageText", percentText);
            SetPrivateField(form, "tipsText", tipsText);
            SetPrivateField(form, "tipsCanvasGroup", tipsCg);
            SetPrivateField(form, "openSoundId", 100001);
            SetPrivateField(form, "finishSoundId", 100002);

            // Save Prefab
            SavePrefab(rootGo);
        }

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("LoginLoadForm", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            Canvas canvas = rootGo.AddComponent<Canvas>();
            canvas.vertexColorAlwaysGammaSpace = true;
            rootGo.AddComponent<CanvasGroup>();
            rootGo.AddComponent<GraphicRaycaster>();
            return rootGo;
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            for (var type = target.GetType(); type != null; type = type.BaseType)
            {
                var field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                if (field != null)
                {
                    field.SetValue(target, value);
                    return;
                }
            }
            Debug.LogError($"[LoginLoadFormCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }

        private static void SavePrefab(GameObject rootGo)
        {
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"LoginLoadForm Prefab successfully generated at: {PrefabPath}");
        }
    }
}
