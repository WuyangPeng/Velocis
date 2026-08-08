using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Menu;

namespace Game.Scripts.Main.Editor.Generator.UI.Form
{
    public static class DialogFormCreator
    {
        private const string PrefabPath = "Assets/Game/UI/UIForms/Dialog/DialogForm.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/dialog_background.png";
        private const string TitleBgSpritePath = "Assets/Game/Textures/Panel/Menu/dialog_title_background.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string ButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";

        private const string ConfirmBtnSpritePath = "Assets/Game/Textures/Button/Menu/dialog_confirm.png";
        private const string CancelBtnSpritePath = "Assets/Game/Textures/Button/Menu/dialog_cancel.png";
        private const string OtherBtnSpritePath = "Assets/Game/Textures/Button/Menu/dialog_other.png";

        // 1920×1080 设计坐标系下的布局尺寸
        private static readonly Vector2 PanelSize = new Vector2(720f, 440f);
        private static readonly Vector2 TitleBgSize = new Vector2(660f, 56f);
        private static readonly Vector2 MessageSize = new Vector2(620f, 150f);
        private const float TitleFontSize = 26f;
        private const float MessageFontSize = 22f;
        private const float DualButtonSpacing = 145f;
        private static readonly Vector2 TripleButtonSize = new Vector2(200f, 60f);
        private const float TripleButtonSpacing = 215f;
        private const float TripleButtonFontSize = 24f;

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

        [MenuItem("Generator/UI/Form/Create Dialog Form Prefab")]
        public static void CreateDialogFormPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Dialog";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Dialog");
            }

            // 配置按钮贴图导入格式为 Sprite
            ConfigureTextureAsSprite(ConfirmBtnSpritePath);
            ConfigureTextureAsSprite(CancelBtnSpritePath);
            ConfigureTextureAsSprite(OtherBtnSpritePath);

            // 加载资源
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            Sprite titleBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(TitleBgSpritePath);
            GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ButtonPrefabPath);

            Sprite confirmBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ConfirmBtnSpritePath);
            Sprite cancelBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CancelBtnSpritePath);
            Sprite otherBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>(OtherBtnSpritePath);

            if (buttonPrefab == null)
            {
                Debug.LogError($"无法加载按钮预制体: {ButtonPrefabPath}");
                return;
            }

            // 创建根节点
            GameObject rootGo = CreateRoot();
            DialogForm dialogForm = rootGo.AddComponent<DialogForm>();

            // 创建 Blocker 背景阻挡
            CreateBlocker(rootGo.transform);

            // 创建主面板
            GameObject bgGo = new GameObject("Background", typeof(RectTransform));
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            RectTransform bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0.5f, 0.5f);
            bgRt.anchorMax = new Vector2(0.5f, 0.5f);
            bgRt.pivot = new Vector2(0.5f, 0.5f);
            bgRt.sizeDelta = PanelSize;
            bgRt.anchoredPosition = Vector2.zero;

            Image bgImage = bgGo.AddComponent<Image>();
            if (bgSprite != null)
            {
                bgImage.sprite = bgSprite;
                bgImage.type = Image.Type.Sliced;
            }
            bgImage.color = Color.white;

            // 创建标题栏
            var titleText = CreateTitle(bgGo.transform, fontAsset, titleBgSprite);

            // 创建消息内容
            var messageText = CreateMessage(bgGo.transform, fontAsset);

            // 创建三个 Mode Button Groups
            GameObject group1Go = new GameObject("ButtonGroup1", typeof(RectTransform));
            group1Go.layer = LayerMask.NameToLayer("UI");
            group1Go.transform.SetParent(bgGo.transform, false);
            ConfigureGroupTransform(group1Go.GetComponent<RectTransform>());

            GameObject group2Go = new GameObject("ButtonGroup2", typeof(RectTransform));
            group2Go.layer = LayerMask.NameToLayer("UI");
            group2Go.transform.SetParent(bgGo.transform, false);
            ConfigureGroupTransform(group2Go.GetComponent<RectTransform>());

            GameObject group3Go = new GameObject("ButtonGroup3", typeof(RectTransform));
            group3Go.layer = LayerMask.NameToLayer("UI");
            group3Go.transform.SetParent(bgGo.transform, false);
            ConfigureGroupTransform(group3Go.GetComponent<RectTransform>());

            // 模式1按钮
            var btnConfirm1 = CreateButton("ConfirmButton", group1Go.transform, buttonPrefab, new Vector2(0, 0), null, confirmBtnSprite);
            SetPrivateField(btnConfirm1, "clickSoundId", 100007);
            SetPrivateField(btnConfirm1, "shortcutKey", KeyCode.Return);
            
            // 模式2按钮（使用 RectangleButton 默认尺寸）
            var btnConfirm2 = CreateButton("ConfirmButton", group2Go.transform, buttonPrefab, new Vector2(DualButtonSpacing, 0), null, confirmBtnSprite);
            var btnCancel2 = CreateButton("CancelButton", group2Go.transform, buttonPrefab, new Vector2(-DualButtonSpacing, 0), null, cancelBtnSprite);
            SetPrivateField(btnConfirm2, "clickSoundId", 100007);
            SetPrivateField(btnConfirm2, "shortcutKey", KeyCode.Return);
            SetPrivateField(btnCancel2, "clickSoundId", 100008);
            SetPrivateField(btnCancel2, "shortcutKey", KeyCode.Escape);

            // 模式3按钮（三按钮模式使用较小尺寸）
            var btnConfirm3 = CreateButton("ConfirmButton", group3Go.transform, buttonPrefab, new Vector2(TripleButtonSpacing, 0), TripleButtonSize, confirmBtnSprite);
            var btnCancel3 = CreateButton("CancelButton", group3Go.transform, buttonPrefab, new Vector2(0, 0), TripleButtonSize, cancelBtnSprite);
            var btnOther3 = CreateButton("OtherButton", group3Go.transform, buttonPrefab, new Vector2(-TripleButtonSpacing, 0), TripleButtonSize, otherBtnSprite);
            SetPrivateField(btnConfirm3, "clickSoundId", 100007);
            SetPrivateField(btnConfirm3, "shortcutKey", KeyCode.Return);
            SetPrivateField(btnCancel3, "clickSoundId", 100008);
            SetPrivateField(btnOther3, "clickSoundId", 100009);
            SetPrivateField(btnCancel3, "shortcutKey", KeyCode.Escape);

            // 挂载按钮事件
            UnityEventTools.AddPersistentListener(btnConfirm1.OnClick, dialogForm.OnConfirmButtonClick);
            UnityEventTools.AddPersistentListener(btnConfirm2.OnClick, dialogForm.OnConfirmButtonClick);
            UnityEventTools.AddPersistentListener(btnCancel2.OnClick, dialogForm.OnCancelButtonClick);
            UnityEventTools.AddPersistentListener(btnConfirm3.OnClick, dialogForm.OnConfirmButtonClick);
            UnityEventTools.AddPersistentListener(btnCancel3.OnClick, dialogForm.OnCancelButtonClick);
            UnityEventTools.AddPersistentListener(btnOther3.OnClick, dialogForm.OnOtherButtonClick);

            // 获取各按钮下的文本
            TMP_Text confirmText1 = btnConfirm1.GetComponentInChildren<TMP_Text>(true);
            TMP_Text confirmText2 = btnConfirm2.GetComponentInChildren<TMP_Text>(true);
            TMP_Text confirmText3 = btnConfirm3.GetComponentInChildren<TMP_Text>(true);

            TMP_Text cancelText2 = btnCancel2.GetComponentInChildren<TMP_Text>(true);
            TMP_Text cancelText3 = btnCancel3.GetComponentInChildren<TMP_Text>(true);

            TMP_Text otherText3 = btnOther3.GetComponentInChildren<TMP_Text>(true);

            // 设置文本默认值 (会由 DialogForm 在 Runtime 覆盖，但保留初始值)
            if (confirmText1 != null)
            {
                confirmText1.text = "确定";
                confirmText1.color = new Color32(60, 40, 20, 255);
            }
            if (confirmText2 != null)
            {
                confirmText2.text = "确定";
                confirmText2.color = new Color32(60, 40, 20, 255);
            }
            if (confirmText3 != null)
            {
                confirmText3.text = "确定";
                confirmText3.color = new Color32(60, 40, 20, 255);
                confirmText3.fontSize = TripleButtonFontSize;
            }
            if (cancelText2 != null)
            {
                cancelText2.text = "取消";
                cancelText2.color = new Color32(80, 30, 30, 255);
            }
            if (cancelText3 != null)
            {
                cancelText3.text = "取消";
                cancelText3.color = new Color32(80, 30, 30, 255);
                cancelText3.fontSize = TripleButtonFontSize;
            }
            if (otherText3 != null)
            {
                otherText3.text = "其他";
                otherText3.color = new Color32(50, 50, 50, 255);
                otherText3.fontSize = TripleButtonFontSize;
            }

            // 反射设置 DialogForm 私有字段
            SetPrivateField(dialogForm, "titleText", titleText);
            SetPrivateField(dialogForm, "messageText", messageText);
            SetPrivateField(dialogForm, "modeObjects", new GameObject[] { group1Go, group2Go, group3Go });
            SetPrivateField(dialogForm, "confirmTexts", new TMP_Text[] { confirmText1, confirmText2, confirmText3 });
            SetPrivateField(dialogForm, "cancelTexts", new TMP_Text[] { cancelText2, cancelText3 });
            SetPrivateField(dialogForm, "otherTexts", new TMP_Text[] { otherText3 });

            // 保存 Prefab
            SavePrefab(rootGo);
        }

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("DialogForm", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");

            RectTransform rt = rootGo.GetComponent<RectTransform>();
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

        private static void CreateBlocker(Transform parent)
        {
            GameObject blockerGo = new GameObject("Blocker", typeof(RectTransform));
            blockerGo.layer = LayerMask.NameToLayer("UI");
            blockerGo.transform.SetParent(parent, false);

            RectTransform rt = blockerGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            Image image = blockerGo.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.4f);
            image.raycastTarget = true;
        }

        private static TMP_Text CreateTitle(Transform parent, TMP_FontAsset fontAsset, Sprite titleBgSprite)
        {
            GameObject titleBgGo = new GameObject("TitleBackground", typeof(RectTransform));
            titleBgGo.layer = LayerMask.NameToLayer("UI");
            titleBgGo.transform.SetParent(parent, false);

            RectTransform bgRt = titleBgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0.5f, 1f);
            bgRt.anchorMax = new Vector2(0.5f, 1f);
            bgRt.pivot = new Vector2(0.5f, 1f);
            bgRt.sizeDelta = TitleBgSize;
            bgRt.anchoredPosition = new Vector2(0f, -10f);

            Image bgImage = titleBgGo.AddComponent<Image>();
            if (titleBgSprite != null)
            {
                bgImage.sprite = titleBgSprite;
                bgImage.type = Image.Type.Sliced;
            }
            bgImage.color = Color.white;

            GameObject textGo = new GameObject("TitleText", typeof(RectTransform));
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(titleBgGo.transform, false);

            RectTransform textRt = textGo.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.anchoredPosition = Vector2.zero;
            textRt.sizeDelta = Vector2.zero;
            textRt.pivot = new Vector2(0.5f, 0.5f);

            TextMeshProUGUI titleText = textGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) titleText.font = fontAsset;
            titleText.text = "提示";
            titleText.fontSize = TitleFontSize;
            titleText.fontStyle = FontStyles.Bold;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.white;

            return titleText;
        }

        private static TMP_Text CreateMessage(Transform parent, TMP_FontAsset fontAsset)
        {
            GameObject textGo = new GameObject("MessageText", typeof(RectTransform));
            textGo.layer = LayerMask.NameToLayer("UI");
            textGo.transform.SetParent(parent, false);

            RectTransform textRt = textGo.GetComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0.5f, 0.5f);
            textRt.anchorMax = new Vector2(0.5f, 0.5f);
            textRt.pivot = new Vector2(0.5f, 0.5f);
            textRt.sizeDelta = MessageSize;
            textRt.anchoredPosition = new Vector2(0f, 15f);

            TextMeshProUGUI messageText = textGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) messageText.font = fontAsset;
            messageText.text = "提示内容";
            messageText.fontSize = MessageFontSize;
            messageText.alignment = TextAlignmentOptions.Center;
            messageText.color = Color.white;
            messageText.enableWordWrapping = true;

            return messageText;
        }

        private static void ConfigureGroupTransform(RectTransform rt)
        {
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(1f, 0.3f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
        }

        private static BaseButton CreateButton(string name, Transform parent, GameObject buttonPrefab, Vector2 anchoredPos, Vector2? size = null, Sprite sprite = null)
        {
            GameObject btnGo = (GameObject)PrefabUtility.InstantiatePrefab(buttonPrefab);
            btnGo.name = name;
            btnGo.transform.SetParent(parent, false);

            RectTransform rt = btnGo.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = anchoredPos;
            if (size.HasValue)
            {
                rt.sizeDelta = size.Value;
            }

            if (sprite != null)
            {
                Transform imgTrans = btnGo.transform.Find("Image");
                Image img = imgTrans != null ? imgTrans.GetComponent<Image>() : btnGo.GetComponentInChildren<Image>(true);
                if (img != null)
                {
                    img.sprite = sprite;
                    img.type = Image.Type.Sliced;
                }
            }

            return btnGo.GetComponent<BaseButton>();
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

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            Debug.Log($"DialogForm 预制体已生成：{PrefabPath}");
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
