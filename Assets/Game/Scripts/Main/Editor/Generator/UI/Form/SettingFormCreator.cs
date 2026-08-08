using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Main;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Main.Editor.Generator.UI.Component;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class SettingFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/SettingForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/setting_background.png";
        private const string DropdownBackgroundSpritePath = "Assets/Game/Textures/Dropdown/Menu/dropdown_background.png";
        private const string DropdownArrowSpritePath = "Assets/Game/Textures/Dropdown/Menu/dropdown_arrow.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string ButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";

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

        // 1920×1080 设计坐标系下的布局尺寸
        private static readonly Vector2 PanelSize = new Vector2(720f, 690f);
        private static readonly Vector2 TitleSize = new Vector2(360f, 56f);
        private const float TitleOffsetY = -75f;
        private const float TitleFontSize = 32f;
        private const float ControlGroupWidth = 620f;
        private const float ControlLabelWidth = 120f;
        private const float ControlLabelFontSize = 22f;
        private const float ControlValueFontSize = 20f;
        private const float VolumeStartY = 140f;
        private const float VolumeStepY = 52f;
        private const float OptionStartY = -20f;
        private const float OptionStepY = 54f;
        private const float ButtonBottomOffset = 40f;

        [MenuItem("Generator/UI/Form/Create Setting Form Prefab")]
        public static void CreateSettingFormPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/System";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "System");
            }

            // 配置背景图导入格式为 Sprite
            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(DropdownBackgroundSpritePath);
            ConfigureTextureAsSprite(DropdownArrowSpritePath);

            // 加载资源
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ButtonPrefabPath);
            GameObject volumeControlPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(VolumeControlCreator.PrefabPath);
            if (volumeControlPrefab == null)
            {
                VolumeControlCreator.CreateVolumeControlPrefab();
                volumeControlPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(VolumeControlCreator.PrefabPath);
            }

            // 创建根节点
            GameObject rootGo = CreateRoot();
            SettingForm settingForm = rootGo.AddComponent<SettingForm>();

            // 1. 创建背景底板
            GameObject bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            RectTransform bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = PanelSize;
            Image bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Sliced;
            }
            bgImg.color = Color.white;

            // 2. 创建标题
            GameObject titleGo = new GameObject("Title");
            titleGo.layer = LayerMask.NameToLayer("UI");
            titleGo.transform.SetParent(bgGo.transform, false);
            RectTransform titleRt = titleGo.AddComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, TitleOffsetY);
            titleRt.sizeDelta = TitleSize;
            TextMeshProUGUI titleText = titleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) titleText.font = fontAsset;
            titleText.text = "SystemSetting.Title";
            titleText.fontSize = TitleFontSize;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0.95f, 0.85f, 0.6f, 1f); // 奢华金色

            // 3. 创建音量滑动条组
            var musicVolumeControl = CreateVolumeControl(bgGo.transform, volumeControlPrefab, "Music", "SystemSetting.Music", new Vector2(0f, VolumeStartY), fontAsset);
            var soundVolumeControl = CreateVolumeControl(bgGo.transform, volumeControlPrefab, "Sound", "SystemSetting.Sound", new Vector2(0f, VolumeStartY - VolumeStepY), fontAsset);
            var uiSoundVolumeControl = CreateVolumeControl(bgGo.transform, volumeControlPrefab, "UISound", "SystemSetting.UISound", new Vector2(0f, VolumeStartY - 2 * VolumeStepY), fontAsset);

            // 4. 创建多语言下拉框
            var languageDropdown = CreateDropdown(bgGo.transform, "LanguageDropdown", "SystemSetting.Language", new Vector2(0f, OptionStartY), fontAsset, new string[] { "简体中文", "繁体中文", "English", "韩文", "日本語" });

            // 5. 创建特效品质下拉框
            var graphicDropdown = CreateDropdown(bgGo.transform, "GraphicQualityDropdown", "SystemSetting.GraphicQuality", new Vector2(0f, OptionStartY - OptionStepY), fontAsset, new string[] { "高", "中", "低" });

            // 6. 创建震动/全屏 ToggleControl 预制体实例
            GameObject toggleControlPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToggleControlCreator.PrefabPath);
            if (toggleControlPrefab == null)
            {
                ToggleControlCreator.CreateToggleControlPrefab();
                toggleControlPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToggleControlCreator.PrefabPath);
            }
            GameObject toggleControlGo = (GameObject)PrefabUtility.InstantiatePrefab(toggleControlPrefab);
            toggleControlGo.name = "VibrationToggleGroup";
            toggleControlGo.transform.SetParent(bgGo.transform, false);
            RectTransform toggleControlRt = toggleControlGo.GetComponent<RectTransform>();
            toggleControlRt.anchoredPosition = new Vector2(0f, OptionStartY - 2 * OptionStepY);
            ToggleControl vibrationToggleControl = toggleControlGo.GetComponent<ToggleControl>();

            // 7. 放置确定/取消按钮（使用 RectangleButton 默认尺寸）
            var (confirmBtn, cancelBtn) = CreateButtons(bgGo.transform, buttonPrefab, fontAsset);

            // 反射绑定私有字段
            SetPrivateField(settingForm, "musicVolumeControl", musicVolumeControl);
            SetPrivateField(settingForm, "soundVolumeControl", soundVolumeControl);
            SetPrivateField(settingForm, "uiSoundVolumeControl", uiSoundVolumeControl);
            SetPrivateField(settingForm, "languageDropdown", languageDropdown);
            SetPrivateField(settingForm, "graphicQualityDropdown", graphicDropdown);
            SetPrivateField(settingForm, "vibrationToggleControl", vibrationToggleControl);
            SetPrivateField(settingForm, "optionSwitchSoundId", 100011);
            SetPrivateField(settingForm, "confirmButton", confirmBtn);
            SetPrivateField(settingForm, "cancelButton", cancelBtn);

            // 序列化绑定事件
            UnityEventTools.AddPersistentListener(musicVolumeControl.MuteToggle.onValueChanged, settingForm.OnMusicMuteChanged);
            UnityEventTools.AddPersistentListener(musicVolumeControl.VolumeSlider.onValueChanged, settingForm.OnMusicVolumeChanged);
            UnityEventTools.AddPersistentListener(soundVolumeControl.MuteToggle.onValueChanged, settingForm.OnSoundMuteChanged);
            UnityEventTools.AddPersistentListener(soundVolumeControl.VolumeSlider.onValueChanged, settingForm.OnSoundVolumeChanged);
            UnityEventTools.AddPersistentListener(uiSoundVolumeControl.MuteToggle.onValueChanged, settingForm.OnUISoundMuteChanged);
            UnityEventTools.AddPersistentListener(uiSoundVolumeControl.VolumeSlider.onValueChanged, settingForm.OnUISoundVolumeChanged);
            UnityEventTools.AddPersistentListener(languageDropdown.Dropdown.onValueChanged, settingForm.OnLanguageChanged);
            UnityEventTools.AddPersistentListener(graphicDropdown.Dropdown.onValueChanged, settingForm.OnGraphicQualityChanged);
            UnityEventTools.AddPersistentListener(vibrationToggleControl.Toggle.onValueChanged, settingForm.OnVibrationChanged);
            UnityEventTools.AddPersistentListener(confirmBtn.OnClick, settingForm.OnConfirmButtonClick);
            UnityEventTools.AddPersistentListener(cancelBtn.OnClick, settingForm.OnCancelButtonClick);

            // 保存 Prefab
            SavePrefab(rootGo);
        }

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("SettingForm");
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.AddComponent<RectTransform>();
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


        private static VolumeControl CreateVolumeControl(Transform parent, GameObject prefab, string prefix, string label, Vector2 pos, TMP_FontAsset fontAsset)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.name = prefix + "VolumeGroup";
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;

            VolumeControl volumeControl = go.GetComponent<VolumeControl>();

            // Find Label
            Transform labelTrans = go.transform.Find("Label");
            if (labelTrans != null)
            {
                TextMeshProUGUI labelTxt = labelTrans.GetComponent<TextMeshProUGUI>();
                if (labelTxt != null)
                {
                    if (fontAsset != null) labelTxt.font = fontAsset;
                    labelTxt.text = label;
                }
            }

            return volumeControl;
        }

        private static DropdownControl CreateDropdown(Transform parent, string name, string label, Vector2 pos, TMP_FontAsset fontAsset, string[] options)
        {
            GameObject dropdownPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DropdownControlCreator.PrefabPath);
            if (dropdownPrefab == null)
            {
                DropdownControlCreator.CreateDropdownControlPrefab();
                dropdownPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DropdownControlCreator.PrefabPath);
            }

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(dropdownPrefab);
            go.name = name + "Group";
            go.transform.SetParent(parent, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;

            // 设置标签
            Transform labelTrans = go.transform.Find("Label");
            if (labelTrans != null)
            {
                TextMeshProUGUI labelTxt = labelTrans.GetComponent<TextMeshProUGUI>();
                if (labelTxt != null)
                {
                    if (fontAsset != null) labelTxt.font = fontAsset;
                    labelTxt.text = label;
                }
            }

            // 获取下拉框组件并配置选项
            Transform dropdownTrans = go.transform.Find("Dropdown");
            TMP_Dropdown dropdown = dropdownTrans != null ? dropdownTrans.GetComponent<TMP_Dropdown>() : go.GetComponentInChildren<TMP_Dropdown>(true);
            if (dropdown != null)
            {
                dropdown.gameObject.name = name;
                dropdown.ClearOptions();
                foreach (var opt in options)
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData(opt));
                }
            }

            return go.GetComponent<DropdownControl>();
        }



        private static (BaseButton confirm, BaseButton cancel) CreateButtons(Transform parent, GameObject buttonPrefab, TMP_FontAsset fontAsset)
        {
            Sprite confirmSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Button/Menu/dialog_confirm.png");
            Sprite cancelSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Game/Textures/Button/Menu/dialog_cancel.png");

            GameObject confirmGo = (GameObject)PrefabUtility.InstantiatePrefab(buttonPrefab);
            confirmGo.name = "ConfirmButton";
            confirmGo.transform.SetParent(parent, false);
            RectTransform confirmRt = confirmGo.GetComponent<RectTransform>();
            confirmRt.anchorMin = new Vector2(0.7f, 0f);
            confirmRt.anchorMax = new Vector2(0.7f, 0f);
            confirmRt.pivot = new Vector2(0.5f, 0f);
            confirmRt.anchoredPosition = new Vector2(0f, ButtonBottomOffset);

            if (confirmSprite != null)
            {
                Transform imgTrans = confirmGo.transform.Find("Image");
                Image img = imgTrans != null ? imgTrans.GetComponent<Image>() : confirmGo.GetComponentInChildren<Image>(true);
                if (img != null)
                {
                    img.sprite = confirmSprite;
                    img.type = Image.Type.Sliced;
                }
            }

            TextMeshProUGUI confirmText = confirmGo.GetComponentInChildren<TextMeshProUGUI>(true);
            if (confirmText != null)
            {
                confirmText.text = "Dialog.ConfirmButton";
                confirmText.color = new Color32(60, 40, 20, 255);
                if (fontAsset != null) confirmText.font = fontAsset;
            }

            BaseButton confirmButton = confirmGo.GetComponent<BaseButton>();
            SetPrivateField(confirmButton, "shortcutKey", KeyCode.Return);

            GameObject cancelGo = (GameObject)PrefabUtility.InstantiatePrefab(buttonPrefab);
            cancelGo.name = "CancelButton";
            cancelGo.transform.SetParent(parent, false);
            RectTransform cancelRt = cancelGo.GetComponent<RectTransform>();
            cancelRt.anchorMin = new Vector2(0.3f, 0f);
            cancelRt.anchorMax = new Vector2(0.3f, 0f);
            cancelRt.pivot = new Vector2(0.5f, 0f);
            cancelRt.anchoredPosition = new Vector2(0f, ButtonBottomOffset);

            if (cancelSprite != null)
            {
                Transform imgTrans = cancelGo.transform.Find("Image");
                Image img = imgTrans != null ? imgTrans.GetComponent<Image>() : cancelGo.GetComponentInChildren<Image>(true);
                if (img != null)
                {
                    img.sprite = cancelSprite;
                    img.type = Image.Type.Sliced;
                }
            }

            TextMeshProUGUI cancelText = cancelGo.GetComponentInChildren<TextMeshProUGUI>(true);
            if (cancelText != null)
            {
                cancelText.text = "Dialog.CancelButton";
                cancelText.color = new Color32(80, 30, 30, 255);
                if (fontAsset != null) cancelText.font = fontAsset;
            }

            BaseButton cancelButton = cancelGo.GetComponent<BaseButton>();
            SetPrivateField(cancelButton, "shortcutKey", KeyCode.Escape);

            return (confirmButton, cancelButton);
        }

        private static void SavePrefab(GameObject rootGo)
        {
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            if (prefabAsset != null)
            {
                Canvas prefabCanvas = prefabAsset.GetComponent<Canvas>();
                if (prefabCanvas != null)
                {
                    prefabCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    prefabCanvas.vertexColorAlwaysGammaSpace = true;
                    prefabCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1
                                                            | AdditionalCanvasShaderChannels.Normal
                                                            | AdditionalCanvasShaderChannels.Tangent;
                }
                EditorUtility.SetDirty(prefabAsset);
            }

            Object.DestroyImmediate(rootGo);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"SettingForm 预制体已生成：{PrefabPath}");
        }

        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            System.Type type = obj.GetType();
            System.Reflection.FieldInfo field = null;
            while (type != null && field == null)
            {
                field = type.GetField(
                    fieldName,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                type = type.BaseType;
            }

            if (field != null)
                field.SetValue(obj, value);
            else
                Debug.LogWarning($"字段 '{fieldName}' 在 {obj.GetType().Name} 及其基类上均未找到！");
        }
    }
}
