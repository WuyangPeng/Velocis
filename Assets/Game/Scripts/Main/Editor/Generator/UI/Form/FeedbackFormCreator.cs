// 创建时间：2026-07-24
// 修改时间：2026-07-24

using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Main;
using Game.Scripts.Main.Editor.Generator.UI.Component;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class FeedbackFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/System/FeedbackForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/setting_background.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string CircleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/CircleButton.prefab";
        private const string CircleCloseSpritePath = "Assets/Game/Textures/Button/Menu/circle_close.png";
        private const string TypeNormalSpritePath = "Assets/Game/Textures/Button/Menu/category_btn_normal.png";
        private const string TypeSelectedSpritePath = "Assets/Game/Textures/Button/Menu/category_btn_selected.png";
        private const string InputFieldBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_bg.png";
        private const string InputFieldShortBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_short_bg.png";
        private const string InputFieldContentBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_content_bg.png";

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

        [MenuItem("Generator/UI/Form/Create Feedback Form Prefab")]
        public static void CreateFeedbackFormPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/System";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "System");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(TypeNormalSpritePath);
            ConfigureTextureAsSprite(TypeSelectedSpritePath);
            ConfigureTextureAsSprite(InputFieldBgSpritePath);
            ConfigureTextureAsSprite(InputFieldShortBgSpritePath);
            ConfigureTextureAsSprite(InputFieldContentBgSpritePath);
            ConfigureTextureAsSprite(CircleCloseSpritePath);

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            var typeNormalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(TypeNormalSpritePath);
            var typeSelectedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(TypeSelectedSpritePath);
            var inputBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldBgSpritePath);
            var inputShortBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldShortBgSpritePath);
            var inputContentBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldContentBgSpritePath);
            var rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);
            var circleButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CircleButtonPrefabPath);

            var rootGo = CreateRoot();
            var form = rootGo.AddComponent<FeedbackForm>();

            var bgGo = new GameObject("Background");
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            var bgRt = bgGo.AddComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(1040f, 800f);
            var bgImg = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImg.sprite = bgSprite;
                bgImg.type = Image.Type.Sliced;
            }

            bgImg.color = Color.white;

            // Title
            var titleGo = new GameObject("Title");
            titleGo.layer = LayerMask.NameToLayer("UI");
            titleGo.transform.SetParent(bgGo.transform, false);
            var titleRt = titleGo.AddComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -80f);
            titleRt.sizeDelta = new Vector2(360f, 48f);
            var titleText = titleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                titleText.font = fontAsset;
            }

            titleText.text = "FeedbackForm.Title";
            titleText.fontSize = 30f;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // Type tabs
            var typePanelGo = new GameObject("TypePanel");
            typePanelGo.layer = LayerMask.NameToLayer("UI");
            typePanelGo.transform.SetParent(bgGo.transform, false);
            var typePanelRt = typePanelGo.AddComponent<RectTransform>();
            typePanelRt.anchorMin = new Vector2(0.5f, 1f);
            typePanelRt.anchorMax = new Vector2(0.5f, 1f);
            typePanelRt.pivot = new Vector2(0.5f, 1f);
            typePanelRt.anchoredPosition = new Vector2(0f, -150f);
            typePanelRt.sizeDelta = new Vector2(920f, 64f);

            var typeButtons = new BaseButton[3];
            var typeLabels = new TMP_Text[3];
            var typeImages = new Image[3];
            var typeKeys = new[] { "Feedback.Type.Bug", "Feedback.Type.Suggestion", "Feedback.Type.Report" };
            var typeSpacing = 310f;
            var startX = -typeSpacing;

            for (var i = 0; i < 3; i++)
            {
                if (rectButtonPrefab == null)
                {
                    break;
                }

                var btnGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                btnGo.name = $"TypeButton_{i}";
                btnGo.transform.SetParent(typePanelGo.transform, false);
                var btnRt = btnGo.GetComponent<RectTransform>();
                btnRt.anchorMin = new Vector2(0.5f, 0.5f);
                btnRt.anchorMax = new Vector2(0.5f, 0.5f);
                btnRt.pivot = new Vector2(0.5f, 0.5f);
                btnRt.anchoredPosition = new Vector2(startX + i * typeSpacing, 0f);
                btnRt.sizeDelta = new Vector2(260f, 60f);

                typeButtons[i] = btnGo.GetComponent<BaseButton>();
                UIEditorCreatorUtility.SetPrivateField(typeButtons[i], "clickSoundId", 100011);
                typeImages[i] = btnGo.transform.Find("Image")?.GetComponent<Image>();
                if (typeImages[i] != null && typeNormalSprite != null)
                {
                    typeImages[i].sprite = typeNormalSprite;
                    typeImages[i].type = Image.Type.Sliced;
                }

                typeLabels[i] = btnGo.GetComponentInChildren<TMP_Text>();
                if (typeLabels[i] != null)
                {
                    typeLabels[i].text = typeKeys[i];
                    typeLabels[i].fontSize = 22f;
                }
            }

            // Content input
            var inputFieldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(InputFieldCreator.PrefabPath);
            GameObject inputGo;
            if (inputFieldPrefab != null)
            {
                inputGo = (GameObject)PrefabUtility.InstantiatePrefab(inputFieldPrefab);
                inputGo.name = "ContentInput";
            }
            else
            {
                inputGo = InputFieldCreator.BuildInputField(fontAsset, inputContentBgSprite != null ? inputContentBgSprite : inputBgSprite, "ContentInput");
            }

            inputGo.transform.SetParent(bgGo.transform, false);
            var inputRt = inputGo.GetComponent<RectTransform>();
            inputRt.anchorMin = new Vector2(0.5f, 1f);
            inputRt.anchorMax = new Vector2(0.5f, 1f);
            inputRt.pivot = new Vector2(0.5f, 1f);
            inputRt.anchoredPosition = new Vector2(0f, -220f);
            inputRt.sizeDelta = new Vector2(920f, 180f);

            if (inputContentBgSprite != null && inputGo.GetComponent<Image>() is Image contentImg)
            {
                contentImg.sprite = inputContentBgSprite;
            }

            var contentInput = inputGo.GetComponent<TMP_InputField>();
            if (contentInput != null)
            {
                contentInput.lineType = TMP_InputField.LineType.MultiLineNewline;
                contentInput.characterLimit = 500;
                if (contentInput.textViewport != null)
                {
                    contentInput.textViewport.anchoredPosition = new Vector2(15f, -4f);
                    contentInput.textViewport.sizeDelta = new Vector2(-75f, -40f);
                }
                if (contentInput.textComponent is TMP_Text contentText)
                {
                    contentText.alignment = TextAlignmentOptions.TopLeft;
                    contentText.color = new Color(0.1f, 0.08f, 0.06f, 1f); // 墨黑/深褐色文字
                }
                if (contentInput.placeholder is TMP_Text placeholder)
                {
                    placeholder.text = "FeedbackForm.InputPlaceholder";
                    placeholder.alignment = TextAlignmentOptions.TopLeft;
                    placeholder.color = new Color(0.35f, 0.28f, 0.2f, 0.8f);
                }
            }

            // Char count
            var charCountGo = new GameObject("CharCount");
            charCountGo.layer = LayerMask.NameToLayer("UI");
            charCountGo.transform.SetParent(bgGo.transform, false);
            var charCountRt = charCountGo.AddComponent<RectTransform>();
            charCountRt.anchorMin = new Vector2(1f, 1f);
            charCountRt.anchorMax = new Vector2(1f, 1f);
            charCountRt.pivot = new Vector2(1f, 1f);
            charCountRt.anchoredPosition = new Vector2(-70f, -410f);
            charCountRt.sizeDelta = new Vector2(160f, 30f);
            var charCountText = charCountGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                charCountText.font = fontAsset;
            }

            charCountText.fontSize = 18f;
            charCountText.alignment = TextAlignmentOptions.Right;
            charCountText.color = new Color(0.95f, 0.85f, 0.6f, 1f);
            charCountText.text = "0 / 500";

            // Player Info Panel
            var infoPanelGo = new GameObject("PlayerInfoPanel");
            infoPanelGo.layer = LayerMask.NameToLayer("UI");
            infoPanelGo.transform.SetParent(bgGo.transform, false);
            var infoPanelRt = infoPanelGo.AddComponent<RectTransform>();
            infoPanelRt.anchorMin = new Vector2(0.5f, 1f);
            infoPanelRt.anchorMax = new Vector2(0.5f, 1f);
            infoPanelRt.pivot = new Vector2(0.5f, 1f);
            infoPanelRt.anchoredPosition = new Vector2(0f, -450f);
            infoPanelRt.sizeDelta = new Vector2(920f, 48f);

            var labeledInputPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(LabeledInputFieldControlCreator.PrefabPath);

            // Server Labeled Input Control
            LabeledInputFieldControl serverControl = null;
            GameObject serverControlGo;
            if (labeledInputPrefab != null)
            {
                serverControlGo = (GameObject)PrefabUtility.InstantiatePrefab(labeledInputPrefab);
                serverControlGo.name = "ServerControlGroup";
            }
            else
            {
                serverControlGo = new GameObject("ServerControlGroup");
                serverControlGo.layer = LayerMask.NameToLayer("UI");
                var scRt = serverControlGo.AddComponent<RectTransform>();
                scRt.sizeDelta = new Vector2(320f, 52f);
                serverControl = serverControlGo.AddComponent<LabeledInputFieldControl>();

                var serverLabelGo = new GameObject("Label");
                serverLabelGo.layer = LayerMask.NameToLayer("UI");
                serverLabelGo.transform.SetParent(serverControlGo.transform, false);
                var serverLabelRt = serverLabelGo.AddComponent<RectTransform>();
                serverLabelRt.anchorMin = new Vector2(0f, 0.5f);
                serverLabelRt.anchorMax = new Vector2(0f, 0.5f);
                serverLabelRt.pivot = new Vector2(0f, 0.5f);
                serverLabelRt.anchoredPosition = Vector2.zero;
                serverLabelRt.sizeDelta = new Vector2(100f, 48f);
                var serverLabelText = serverLabelGo.AddComponent<TextMeshProUGUI>();
                if (fontAsset != null) serverLabelText.font = fontAsset;
                serverLabelText.text = "FeedbackForm.ServerLabel";
                serverLabelText.fontSize = 22f;
                serverLabelText.enableWordWrapping = false;
                serverLabelText.alignment = TextAlignmentOptions.Left;
                serverLabelText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

                GameObject serverInputGo = inputFieldPrefab != null
                    ? (GameObject)PrefabUtility.InstantiatePrefab(inputFieldPrefab)
                    : InputFieldCreator.BuildInputField(fontAsset, inputShortBgSprite != null ? inputShortBgSprite : inputBgSprite, "InputField");

                serverInputGo.name = "InputField";
                serverInputGo.transform.SetParent(serverControlGo.transform, false);
                var serverInputRt = serverInputGo.GetComponent<RectTransform>();
                serverInputRt.anchorMin = new Vector2(0f, 0.5f);
                serverInputRt.anchorMax = new Vector2(0f, 0.5f);
                serverInputRt.pivot = new Vector2(0f, 0.5f);
                serverInputRt.anchoredPosition = new Vector2(105f, 0f);
                serverInputRt.sizeDelta = new Vector2(210f, 52f);
                if (inputShortBgSprite != null && serverInputGo.GetComponent<Image>() is Image serverImg)
                {
                    serverImg.sprite = inputShortBgSprite;
                }

                var serverInput = serverInputGo.GetComponent<TMP_InputField>();
                if (serverInput != null)
                {
                    serverInput.contentType = TMP_InputField.ContentType.IntegerNumber;
                    if (serverInput.textViewport != null)
                    {
                        serverInput.textViewport.anchoredPosition = Vector2.zero;
                        serverInput.textViewport.sizeDelta = new Vector2(-28f, -11f);
                    }
                    if (serverInput.textComponent is TMP_Text serverText)
                    {
                        serverText.enableWordWrapping = false;
                        serverText.color = new Color(0.05f, 0.2f, 0.25f, 1f);
                    }
                    if (serverInput.placeholder is TMP_Text serverPlaceholder)
                    {
                        serverPlaceholder.text = "FeedbackForm.ServerPlaceholder";
                        serverPlaceholder.enableWordWrapping = false;
                        serverPlaceholder.color = new Color(0.15f, 0.35f, 0.4f, 0.85f);
                    }
                }

                UIEditorCreatorUtility.SetPrivateField(serverControl, "labelText", serverLabelText);
                UIEditorCreatorUtility.SetPrivateField(serverControl, "inputField", serverInput);
            }

            serverControlGo.transform.SetParent(infoPanelGo.transform, false);
            var serverControlRt = serverControlGo.GetComponent<RectTransform>();
            serverControlRt.anchorMin = new Vector2(0f, 0.5f);
            serverControlRt.anchorMax = new Vector2(0f, 0.5f);
            serverControlRt.pivot = new Vector2(0f, 0.5f);
            serverControlRt.anchoredPosition = Vector2.zero;
            serverControlRt.sizeDelta = new Vector2(320f, 52f);
            if (serverControl == null)
            {
                serverControl = serverControlGo.GetComponent<LabeledInputFieldControl>();
            }

            // Player ID Labeled Input Control
            LabeledInputFieldControl playerIdControl = null;
            GameObject playerIdControlGo;
            if (labeledInputPrefab != null)
            {
                playerIdControlGo = (GameObject)PrefabUtility.InstantiatePrefab(labeledInputPrefab);
                playerIdControlGo.name = "PlayerIdControlGroup";
            }
            else
            {
                playerIdControlGo = new GameObject("PlayerIdControlGroup");
                playerIdControlGo.layer = LayerMask.NameToLayer("UI");
                var pcRt = playerIdControlGo.AddComponent<RectTransform>();
                pcRt.sizeDelta = new Vector2(320f, 52f);
                playerIdControl = playerIdControlGo.AddComponent<LabeledInputFieldControl>();

                var playerIdLabelGo = new GameObject("Label");
                playerIdLabelGo.layer = LayerMask.NameToLayer("UI");
                playerIdLabelGo.transform.SetParent(playerIdControlGo.transform, false);
                var playerIdLabelRt = playerIdLabelGo.AddComponent<RectTransform>();
                playerIdLabelRt.anchorMin = new Vector2(0f, 0.5f);
                playerIdLabelRt.anchorMax = new Vector2(0f, 0.5f);
                playerIdLabelRt.pivot = new Vector2(0f, 0.5f);
                playerIdLabelRt.anchoredPosition = Vector2.zero;
                playerIdLabelRt.sizeDelta = new Vector2(110f, 48f);
                var playerIdLabelText = playerIdLabelGo.AddComponent<TextMeshProUGUI>();
                if (fontAsset != null) playerIdLabelText.font = fontAsset;
                playerIdLabelText.text = "FeedbackForm.PlayerIdLabel";
                playerIdLabelText.fontSize = 22f;
                playerIdLabelText.enableWordWrapping = false;
                playerIdLabelText.alignment = TextAlignmentOptions.Left;
                playerIdLabelText.color = new Color(0.95f, 0.85f, 0.6f, 1f);

                GameObject playerIdInputGo = inputFieldPrefab != null
                    ? (GameObject)PrefabUtility.InstantiatePrefab(inputFieldPrefab)
                    : InputFieldCreator.BuildInputField(fontAsset, inputShortBgSprite != null ? inputShortBgSprite : inputBgSprite, "InputField");

                playerIdInputGo.name = "InputField";
                playerIdInputGo.transform.SetParent(playerIdControlGo.transform, false);
                var playerIdInputRt = playerIdInputGo.GetComponent<RectTransform>();
                playerIdInputRt.anchorMin = new Vector2(0f, 0.5f);
                playerIdInputRt.anchorMax = new Vector2(0f, 0.5f);
                playerIdInputRt.pivot = new Vector2(0f, 0.5f);
                playerIdInputRt.anchoredPosition = new Vector2(105f, 0f);
                playerIdInputRt.sizeDelta = new Vector2(210f, 52f);
                if (inputShortBgSprite != null && playerIdInputGo.GetComponent<Image>() is Image playerImg)
                {
                    playerImg.sprite = inputShortBgSprite;
                }

                var playerIdInput = playerIdInputGo.GetComponent<TMP_InputField>();
                if (playerIdInput != null)
                {
                    playerIdInput.contentType = TMP_InputField.ContentType.IntegerNumber;
                    if (playerIdInput.textViewport != null)
                    {
                        playerIdInput.textViewport.anchoredPosition = Vector2.zero;
                        playerIdInput.textViewport.sizeDelta = new Vector2(-28f, -11f);
                    }
                    if (playerIdInput.textComponent is TMP_Text playerIdText)
                    {
                        playerIdText.enableWordWrapping = false;
                        playerIdText.color = new Color(0.05f, 0.2f, 0.25f, 1f);
                    }
                    if (playerIdInput.placeholder is TMP_Text playerIdPlaceholder)
                    {
                        playerIdPlaceholder.text = "FeedbackForm.PlayerIdPlaceholder";
                        playerIdPlaceholder.enableWordWrapping = false;
                        playerIdPlaceholder.color = new Color(0.15f, 0.35f, 0.4f, 0.85f);
                    }
                }

                UIEditorCreatorUtility.SetPrivateField(playerIdControl, "labelText", playerIdLabelText);
                UIEditorCreatorUtility.SetPrivateField(playerIdControl, "inputField", playerIdInput);
            }

            playerIdControlGo.transform.SetParent(infoPanelGo.transform, false);
            var playerIdControlRt = playerIdControlGo.GetComponent<RectTransform>();
            playerIdControlRt.anchorMin = new Vector2(0f, 0.5f);
            playerIdControlRt.anchorMax = new Vector2(0f, 0.5f);
            playerIdControlRt.pivot = new Vector2(0f, 0.5f);
            playerIdControlRt.anchoredPosition = new Vector2(335f, 0f);
            playerIdControlRt.sizeDelta = new Vector2(320f, 52f);
            if (playerIdControl == null)
            {
                playerIdControl = playerIdControlGo.GetComponent<LabeledInputFieldControl>();
            }

            // Anonymous Toggle
            ToggleControl anonymousToggle = null;
            var togglePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ToggleControlCreator.PrefabPath);
            if (togglePrefab != null)
            {
                var toggleGo = (GameObject)PrefabUtility.InstantiatePrefab(togglePrefab);
                toggleGo.name = "AnonymousToggle";
                toggleGo.transform.SetParent(infoPanelGo.transform, false);
                var toggleRt = toggleGo.GetComponent<RectTransform>();
                toggleRt.anchorMin = new Vector2(1f, 0.5f);
                toggleRt.anchorMax = new Vector2(1f, 0.5f);
                toggleRt.pivot = new Vector2(1f, 0.5f);
                toggleRt.anchoredPosition = new Vector2(0f, 0f);
                toggleRt.sizeDelta = new Vector2(180f, 48f);
                anonymousToggle = toggleGo.GetComponent<ToggleControl>();
                if (anonymousToggle != null)
                {
                    if (anonymousToggle.LabelText != null)
                    {
                        anonymousToggle.LabelText.text = "FeedbackForm.AnonymousLabel";
                        anonymousToggle.LabelText.fontSize = 20f;
                        anonymousToggle.LabelText.color = new Color(0.95f, 0.85f, 0.6f, 1f);
                    }
                    if (anonymousToggle.Toggle != null)
                    {
                        anonymousToggle.Toggle.isOn = false;
                    }
                }
            }

            // Screenshot area
            var shotPanelGo = new GameObject("ScreenshotPanel");
            shotPanelGo.layer = LayerMask.NameToLayer("UI");
            shotPanelGo.transform.SetParent(bgGo.transform, false);
            var shotPanelRt = shotPanelGo.AddComponent<RectTransform>();
            shotPanelRt.anchorMin = new Vector2(0f, 0f);
            shotPanelRt.anchorMax = new Vector2(0f, 0f);
            shotPanelRt.pivot = new Vector2(0f, 0f);
            shotPanelRt.anchoredPosition = new Vector2(70f, 120f);
            shotPanelRt.sizeDelta = new Vector2(200f, 140f);

            var shotBg = shotPanelGo.AddComponent<Image>();
            shotBg.color = new Color(0.2f, 0.15f, 0.1f, 0.35f);

            var placeholderGo = new GameObject("Placeholder");
            placeholderGo.layer = LayerMask.NameToLayer("UI");
            placeholderGo.transform.SetParent(shotPanelGo.transform, false);
            var placeholderRt = placeholderGo.AddComponent<RectTransform>();
            placeholderRt.anchorMin = Vector2.zero;
            placeholderRt.anchorMax = Vector2.one;
            placeholderRt.sizeDelta = Vector2.zero;
            var placeholderTxt = placeholderGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                placeholderTxt.font = fontAsset;
            }

            placeholderTxt.text = "FeedbackForm.UploadHint";
            placeholderTxt.fontSize = 18f;
            placeholderTxt.alignment = TextAlignmentOptions.Center;
            placeholderTxt.color = new Color(0.95f, 0.85f, 0.6f, 0.8f);

            var previewGo = new GameObject("Preview");
            previewGo.layer = LayerMask.NameToLayer("UI");
            previewGo.transform.SetParent(shotPanelGo.transform, false);
            var previewRt = previewGo.AddComponent<RectTransform>();
            previewRt.anchorMin = Vector2.zero;
            previewRt.anchorMax = Vector2.one;
            previewRt.offsetMin = new Vector2(6f, 6f);
            previewRt.offsetMax = new Vector2(-6f, -6f);
            var previewRaw = previewGo.AddComponent<RawImage>();
            previewRaw.color = Color.white;
            previewGo.SetActive(false);

            BaseButton uploadButton = null;
            if (rectButtonPrefab != null)
            {
                var uploadGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                uploadGo.name = "UploadButton";
                uploadGo.transform.SetParent(bgGo.transform, false);
                var uploadRt = uploadGo.GetComponent<RectTransform>();
                uploadRt.anchorMin = new Vector2(0f, 0f);
                uploadRt.anchorMax = new Vector2(0f, 0f);
                uploadRt.pivot = new Vector2(0f, 0f);
                uploadRt.anchoredPosition = new Vector2(290f, 155f);
                uploadRt.sizeDelta = new Vector2(180f, 56f);
                uploadButton = uploadGo.GetComponent<BaseButton>();
                UIEditorCreatorUtility.SetPrivateField(uploadButton, "clickSoundId", 100004);
                var uploadTxt = uploadGo.GetComponentInChildren<TMP_Text>();
                if (uploadTxt != null)
                {
                    uploadTxt.text = "FeedbackForm.BtnUpload";
                }
            }

            BaseButton deleteImageButton = null;
            if (circleButtonPrefab != null)
            {
                var deleteGo = (GameObject)PrefabUtility.InstantiatePrefab(circleButtonPrefab);
                deleteGo.name = "DeleteImageButton";
                deleteGo.transform.SetParent(shotPanelGo.transform, false);
                var deleteRt = deleteGo.GetComponent<RectTransform>();
                deleteRt.anchorMin = new Vector2(1f, 1f);
                deleteRt.anchorMax = new Vector2(1f, 1f);
                deleteRt.pivot = new Vector2(1f, 1f);
                deleteRt.anchoredPosition = new Vector2(10f, 10f);
                deleteRt.sizeDelta = new Vector2(48f, 48f);
                var circleBtn = deleteGo.GetComponent<CircleButton>();
                if (circleBtn != null)
                {
                    circleBtn.SetTextActive(false);
                }

                deleteImageButton = circleBtn;
                var closeImage = deleteGo.transform.Find("Image")?.GetComponent<Image>();
                var closeSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CircleCloseSpritePath);
                if (closeImage != null && closeSprite != null)
                {
                    closeImage.sprite = closeSprite;
                }

                deleteGo.SetActive(false);
            }

            // Bottom buttons
            BaseButton submitButton = null;
            TMP_Text submitButtonText = null;
            BaseButton cancelButton = null;
            if (rectButtonPrefab != null)
            {
                var submitGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                submitGo.name = "SubmitButton";
                submitGo.transform.SetParent(bgGo.transform, false);
                var submitRt = submitGo.GetComponent<RectTransform>();
                submitRt.anchorMin = new Vector2(1f, 0f);
                submitRt.anchorMax = new Vector2(1f, 0f);
                submitRt.pivot = new Vector2(1f, 0f);
                submitRt.anchoredPosition = new Vector2(-70f, 40f);
                submitRt.sizeDelta = new Vector2(200f, 64f);
                submitButton = submitGo.GetComponent<BaseButton>();
                UIEditorCreatorUtility.SetPrivateField(submitButton, "clickSoundId", 100019);
                UIEditorCreatorUtility.SetPrivateField(submitButton, "shortcutKey", KeyCode.Return);
                submitButtonText = submitGo.GetComponentInChildren<TMP_Text>();
                if (submitButtonText != null)
                {
                    submitButtonText.text = "FeedbackForm.BtnSubmit";
                }

                if (submitGo.GetComponent<CanvasGroup>() == null)
                {
                    submitGo.AddComponent<CanvasGroup>();
                }

                var cancelGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                cancelGo.name = "CancelButton";
                cancelGo.transform.SetParent(bgGo.transform, false);
                var cancelRt = cancelGo.GetComponent<RectTransform>();
                cancelRt.anchorMin = new Vector2(1f, 0f);
                cancelRt.anchorMax = new Vector2(1f, 0f);
                cancelRt.pivot = new Vector2(1f, 0f);
                cancelRt.anchoredPosition = new Vector2(-290f, 40f);
                cancelRt.sizeDelta = new Vector2(200f, 64f);
                cancelButton = cancelGo.GetComponent<BaseButton>();
                UIEditorCreatorUtility.SetPrivateField(cancelButton, "clickSoundId", 100020);
                UIEditorCreatorUtility.SetPrivateField(cancelButton, "shortcutKey", KeyCode.Escape);
                var cancelTxt = cancelGo.GetComponentInChildren<TMP_Text>();
                if (cancelTxt != null)
                {
                    cancelTxt.text = "FeedbackForm.BtnCancel";
                }
            }

            UIEditorCreatorUtility.SetPrivateField(form, "titleText", titleText);
            UIEditorCreatorUtility.SetPrivateField(form, "typeButtons", typeButtons);
            UIEditorCreatorUtility.SetPrivateField(form, "typeButtonLabels", typeLabels);
            UIEditorCreatorUtility.SetPrivateField(form, "typeButtonImages", typeImages);
            UIEditorCreatorUtility.SetPrivateField(form, "typeNormalSprite", typeNormalSprite);
            UIEditorCreatorUtility.SetPrivateField(form, "typeSelectedSprite", typeSelectedSprite);
            UIEditorCreatorUtility.SetPrivateField(form, "contentInput", contentInput);
            UIEditorCreatorUtility.SetPrivateField(form, "charCountText", charCountText);
            UIEditorCreatorUtility.SetPrivateField(form, "uploadButton", uploadButton);
            UIEditorCreatorUtility.SetPrivateField(form, "deleteImageButton", deleteImageButton);
            UIEditorCreatorUtility.SetPrivateField(form, "screenshotPreview", previewRaw);
            UIEditorCreatorUtility.SetPrivateField(form, "screenshotPlaceholder", placeholderGo);
            UIEditorCreatorUtility.SetPrivateField(form, "submitButton", submitButton);
            UIEditorCreatorUtility.SetPrivateField(form, "submitButtonText", submitButtonText);
            UIEditorCreatorUtility.SetPrivateField(form, "cancelButton", cancelButton);
            UIEditorCreatorUtility.SetPrivateField(form, "serverControl", serverControl);
            UIEditorCreatorUtility.SetPrivateField(form, "playerIdControl", playerIdControl);
            UIEditorCreatorUtility.SetPrivateField(form, "anonymousToggle", anonymousToggle);
            UIEditorCreatorUtility.SetPrivateField(form, "openSoundId", 100018);
            UIEditorCreatorUtility.SetPrivateField(form, "submitSoundId", 100019);
            UIEditorCreatorUtility.SetPrivateField(form, "closeSoundId", 100020);
            UIEditorCreatorUtility.SetPrivateField(form, "cooldownSoundId", 100021);

            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"FeedbackForm Prefab successfully generated at: {PrefabPath}");
        }

        private static GameObject CreateRoot()
        {
            var rootGo = new GameObject("FeedbackForm");
            rootGo.layer = LayerMask.NameToLayer("UI");
            var rt = rootGo.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            var canvas = rootGo.AddComponent<Canvas>();
            canvas.vertexColorAlwaysGammaSpace = true;
            rootGo.AddComponent<CanvasGroup>();
            rootGo.AddComponent<GraphicRaycaster>();
            return rootGo;
        }
    }
}
