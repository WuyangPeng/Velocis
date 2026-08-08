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
    public static class MainEntryFormCreator
    {
        private const string PrefabPath             = "Assets/Game/UI/UIForms/Main/MainEntryForm.prefab";
        private const string RectangleSpritePath    = "Assets/Game/Textures/Button/Menu/rectangle_default.png";
        private const string AccountPanelBgSpritePath = "Assets/Game/Textures/Panel/Menu/account_panel_bg.png";
        private const string InputFieldBgSpritePath = "Assets/Game/Textures/Input/Menu/input_field_bg.png";
        private const string FontPath               = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string QuitSpritePath         = "Assets/Game/Textures/Button/Menu/quit_button_default.png";
        private const string EnterGameBtnBgSpritePath = "Assets/Game/Textures/Button/Menu/enter_game_btn_bg.png";
        private const string LoadingSpinnerSpritePath = "Assets/Game/Textures/Button/Menu/loading_spinner.png";

        // 1920×1080 设计坐标系下的布局尺寸
        private static readonly Vector2 GameTitleSize = new Vector2(900f, 112f);
        private const float GameTitleFontSize = 72f;
        private const float GameTitleTopOffset = -45f;

        private static readonly Vector2 AccountPanelSize = new Vector2(790f, 384f);
        private const float AccountPanelAnchorX = 0.36f;
        private const float InputFieldTopOffset = 69f;

        private static readonly Vector2 EnterGameButtonSize = new Vector2(520f, 104f);
        private const float EnterGameButtonBottomOffset = 47f;
        private static readonly Vector2 LoadingSpinnerSize = new Vector2(108f, 108f);

        private static readonly Vector2 RightPanelSize = new Vector2(290f, 460f);
        private const float RightPanelAnchorX = 0.68f;
        private const float MenuButtonSpacing = 105f;

        private const float QuitButtonMargin = 40f;

        private static readonly Vector2 BottomPanelSize = new Vector2(1100f, 160f);
        private const float AdviceFontSize = 24f;
        private const float CopyrightFontSize = 20f;
        private const float AdviceLineSpacing = 8f;
        private const float CopyrightBottomOffset = 14f;
        private const float CopyrightTextHeight = 30f;
        private const float AdviceCopyrightGap = 16f;
        private const float AdviceTextHeight = 80f;

        private const float VersionFontSize = 20f;

        // ──────────────────────────────────────────────
        // 编辑器菜单入口
        // ──────────────────────────────────────────────

        [MenuItem("Generator/UI/Form/Create Main Entry Form Prefab")]
        public static void CreateMainEntryFormPrefab()
        {
            // 确保目标目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Main";
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Main");

            // 加载公共资源
            TMP_FontAsset fontAsset            = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite        rectSprite           = AssetDatabase.LoadAssetAtPath<Sprite>(RectangleSpritePath);
            Sprite        accountPanelBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(AccountPanelBgSpritePath);
            Sprite        enterGameBtnBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(EnterGameBtnBgSpritePath);

            // 创建根节点
            GameObject rootGo = CreateRoot();
            MainEntryForm mainEntryForm = rootGo.AddComponent<MainEntryForm>();

            // 创建各子节点
            CreateGameTitle(rootGo.transform, fontAsset);
            var (inputField, accountPanelGo) = CreateAccountPanel(rootGo.transform, fontAsset, accountPanelBgSprite);
            var (enterGameButton, spinnerGo, enterGameButtonText) = CreateEnterGameButton(accountPanelGo.transform, fontAsset, enterGameBtnBgSprite);
            var (settingButton, aboutButton, announcementButton, serviceButton) = CreateRightPanel(rootGo.transform, fontAsset, rectSprite);
            var quitButton                   = CreateQuitButton(rootGo.transform, fontAsset);
            CreateBottomPanel(rootGo.transform, fontAsset);
            var versionText                  = CreateVersionText(rootGo.transform, fontAsset);

            // 退出按钮移至最后，确保渲染层级在 BottomPanel 之上
            quitButton.gameObject.transform.SetAsLastSibling();

            // 通过反射将字段绑定到 MainEntryForm
            SetPrivateField(mainEntryForm, "accountInputField",   inputField);
            SetPrivateField(mainEntryForm, "enterGameButton",     enterGameButton);
            SetPrivateField(mainEntryForm, "enterGameButtonText", enterGameButtonText);
            SetPrivateField(mainEntryForm, "settingButton",       settingButton);
            SetPrivateField(mainEntryForm, "aboutButton",         aboutButton);
            SetPrivateField(mainEntryForm, "announcementButton",  announcementButton);
            RedDot redDotComp = announcementButton != null ? announcementButton.transform.Find("RedDot")?.GetComponent<RedDot>() : null;
            SetPrivateField(mainEntryForm, "announcementRedDot",  redDotComp);
            SetPrivateField(mainEntryForm, "serviceButton",       serviceButton);
            SetPrivateField(mainEntryForm, "quitButton",          quitButton);
            SetPrivateField(mainEntryForm, "versionText",         versionText);
            SetPrivateField(mainEntryForm, "loadingSpinner",      spinnerGo); 

            // 挂载进入游戏按钮的 OnEnterGameButtonClick 回调事件至预制体序列化
            if (enterGameButton != null)
            {
                SetPrivateField(enterGameButton, "shortcutKey", KeyCode.Return);
                UnityEventTools.AddPersistentListener(enterGameButton.OnClick, mainEntryForm.OnEnterGameButtonClick);
            }

            // 挂载退出按钮的 OnQuitButtonClick 回调事件至预制体序列化
            if (quitButton != null)
            {
                SetPrivateField(quitButton, "shortcutKey", KeyCode.Escape);
                UnityEventTools.AddPersistentListener(quitButton.OnClick, mainEntryForm.OnQuitButtonClick);
            }

            // 挂载设置按钮的 OnSettingButtonClick 回调事件至预制体序列化
            if (settingButton != null)
            {
                UnityEventTools.AddPersistentListener(settingButton.OnClick, mainEntryForm.OnSettingButtonClick);
            }

            // 挂载关于/九州志按钮的 OnAboutButtonClick 回调事件至预制体序列化
            if (aboutButton != null)
            {
                UnityEventTools.AddPersistentListener(aboutButton.OnClick, mainEntryForm.OnAboutButtonClick);
            }

            // 挂载安民告示按钮的 OnAnnouncementButtonClick 回调事件至预制体序列化
            if (announcementButton != null)
            {
                UnityEventTools.AddPersistentListener(announcementButton.OnClick, mainEntryForm.OnAnnouncementButtonClick);
            }

            // 挂载求助军师/问题反馈按钮的 OnServiceButtonClick 回调事件至预制体序列化
            if (serviceButton != null)
            {
                UnityEventTools.AddPersistentListener(serviceButton.OnClick, mainEntryForm.OnServiceButtonClick);
            }

            // 保存为 Prefab
            SavePrefab(rootGo);
        }

        // ──────────────────────────────────────────────
        // 根节点
        // ──────────────────────────────────────────────

        private static GameObject CreateRoot()
        {
            GameObject rootGo = new GameObject("MainEntryForm");
            rootGo.layer = LayerMask.NameToLayer("UI");

            RectTransform rt = rootGo.AddComponent<RectTransform>();
            rt.anchorMin       = Vector2.zero;
            rt.anchorMax       = Vector2.one;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta       = Vector2.zero;
            rt.pivot           = new Vector2(0.5f, 0.5f);

            // 解决 Linear 空间下 UI 顶点颜色精度问题
            var canvas = rootGo.AddComponent<Canvas>();
            canvas.vertexColorAlwaysGammaSpace = true;

            rootGo.AddComponent<CanvasGroup>();
            rootGo.AddComponent<GraphicRaycaster>();

            return rootGo;
        }

        // ──────────────────────────────────────────────
        // 游戏大标题
        // ──────────────────────────────────────────────

        private static void CreateGameTitle(Transform parent, TMP_FontAsset fontAsset)
        {
            GameObject go = new GameObject("GameTitle");
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(parent, false);

            RectTransform rt = go.AddComponent<RectTransform>();
            rt.anchorMin       = new Vector2(0.5f, 1f);
            rt.anchorMax       = new Vector2(0.5f, 1f);
            rt.pivot           = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, GameTitleTopOffset);
            rt.sizeDelta       = GameTitleSize;

            TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) text.font = fontAsset;
            text.text       = "MainEntry.Title"; // 本地化 Key
            text.fontSize   = GameTitleFontSize;
            text.fontStyle  = FontStyles.Bold;
            text.alignment  = TextAlignmentOptions.Top;
            text.color      = new Color(0.95f, 0.85f, 0.6f, 1f); // 奢华金色
        }

        // ──────────────────────────────────────────────
        // 左侧账号输入面板
        // ──────────────────────────────────────────────

        private static (TMP_InputField inputField, GameObject panelGo) CreateAccountPanel(
            Transform parent, TMP_FontAsset fontAsset, Sprite panelBgSprite)
        {
            // 面板容器
            GameObject panelGo = new GameObject("AccountPanel");
            panelGo.layer = LayerMask.NameToLayer("UI");
            panelGo.transform.SetParent(parent, false);

            RectTransform panelRt = panelGo.AddComponent<RectTransform>();
            panelRt.anchorMin       = new Vector2(AccountPanelAnchorX, 0.5f);
            panelRt.anchorMax       = new Vector2(AccountPanelAnchorX, 0.5f);
            panelRt.pivot           = new Vector2(0.5f, 0.5f);
            panelRt.sizeDelta       = AccountPanelSize;
            panelRt.anchoredPosition = new Vector2(0f, 0f);

            Image panelBg = panelGo.AddComponent<Image>();
            if (panelBgSprite != null) panelBg.sprite = panelBgSprite;
            panelBg.color = Color.white;

            // 账号输入框
            TMP_InputField inputField = CreateInputField(panelGo.transform, fontAsset);

            return (inputField, panelGo);
        }

        // ──────────────────────────────────────────────
        // 账号输入框（复用 InputField 预制体）
        // ──────────────────────────────────────────────

        private static TMP_InputField CreateInputField(Transform parent, TMP_FontAsset fontAsset)
        {
            // 优先加载 InputField 预制体
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(InputFieldCreator.PrefabPath);
            GameObject go;
            if (prefab != null)
            {
                go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.LogWarning("InputField prefab 未找到，手动构建。请先执行 Generator/UI/Form/Create InputField Prefab。");
                // 加载备用资源，由 InputFieldCreator 统一构建
                TMP_FontAsset fallbackFont = fontAsset ?? AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
                Sprite        fallbackBg   = AssetDatabase.LoadAssetAtPath<Sprite>(InputFieldBgSpritePath);
                go = InputFieldCreator.BuildInputField(fallbackFont, fallbackBg);
            }

            go.name = "AccountInputField";
            go.transform.SetParent(parent, false);

            // 配置 RectTransform（位置由 AccountPanel 决定）
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin        = new Vector2(0.5f, 1f);
            rt.anchorMax        = new Vector2(0.5f, 1f);
            rt.pivot            = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -InputFieldTopOffset);

            // 设置占位提示文本的本地化 Key
            TMP_InputField inputField = go.GetComponent<TMP_InputField>();
            if (inputField != null && inputField.placeholder is TextMeshProUGUI placeholder)
                placeholder.text = "MainEntry.InputPlaceholder";

            // 同步字体（预制体可能使用不同字体）
            if (fontAsset != null)
            {
                foreach (var txt in go.GetComponentsInChildren<TextMeshProUGUI>(true))
                    txt.font = fontAsset;
            }

            return go.GetComponent<TMP_InputField>();
        }

        // ──────────────────────────────────────────────
        // 进入游戏按钮 + 加载菊花
        // ──────────────────────────────────────────────

        private static (BaseButton enterGameButton, GameObject spinnerGo, TextMeshProUGUI enterGameButtonText) CreateEnterGameButton(
            Transform parent, TMP_FontAsset fontAsset, Sprite btnBgSprite)
        {
            // 优先复用通用 RectangleButton 预制体
            GameObject btnPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab");
            GameObject btnGo;
            if (btnPrefab != null)
            {
                btnGo = (GameObject)PrefabUtility.InstantiatePrefab(btnPrefab);
            }
            else
            {
                Debug.LogWarning("RectangleButton prefab 未找到，手动创建 EnterGameButton。");
                btnGo = new GameObject("EnterGameButton");
                btnGo.layer = LayerMask.NameToLayer("UI");
                btnGo.AddComponent<RectangleButton>();

                GameObject textGo = new GameObject("Text");
                textGo.layer = LayerMask.NameToLayer("UI");
                textGo.transform.SetParent(btnGo.transform, false);
                TextMeshProUGUI txt = textGo.AddComponent<TextMeshProUGUI>();
                if (fontAsset != null) txt.font = fontAsset;
                txt.text      = "MainEntry.BtnEnterGame";
                txt.fontSize  = 32;
                txt.alignment = TextAlignmentOptions.Center;
                txt.color     = new Color(0.95f, 0.9f, 0.75f, 1f);
                RectTransform txtRt = txt.GetComponent<RectTransform>();
                txtRt.anchorMin       = Vector2.zero;
                txtRt.anchorMax       = Vector2.one;
                txtRt.anchoredPosition = Vector2.zero;
                txtRt.sizeDelta       = Vector2.zero;
            }

            // 获取实例化出来的 Image 组件（若是预制体，通常在子物体 Image 上），并应用新背景图
            Image btnBg = btnGo.GetComponentInChildren<Image>(true);
            if (btnBg == null)
            {
                btnBg = btnGo.AddComponent<Image>();
            }
            if (btnBgSprite != null)
            {
                btnBg.sprite = btnBgSprite;
            }
            btnBg.color = Color.white;

            btnGo.name = "EnterGameButton";
            btnGo.transform.SetParent(parent, false);

            RectTransform rt = btnGo.GetComponent<RectTransform>();
            rt.anchorMin       = new Vector2(0.5f, 0f);
            rt.anchorMax       = new Vector2(0.5f, 0f);
            rt.pivot           = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(0f, EnterGameButtonBottomOffset);
            rt.sizeDelta       = EnterGameButtonSize;

            // 同步按钮文本与字体
            TextMeshProUGUI btnText = btnGo.GetComponentInChildren<TextMeshProUGUI>(true);
            if (btnText != null)
            {
                btnText.text = "MainEntry.BtnEnterGame";
                if (fontAsset != null) btnText.font = fontAsset;
            }

            BaseButton enterGameButton = btnGo.GetComponent<BaseButton>();

            // 加载中菊花（默认隐藏，靠右对齐）
            GameObject spinnerGo = new GameObject("LoadingSpinner");
            spinnerGo.layer = LayerMask.NameToLayer("UI");
            spinnerGo.transform.SetParent(btnGo.transform, false);
            spinnerGo.SetActive(false);

            RectTransform spinnerRt = spinnerGo.AddComponent<RectTransform>();
            spinnerRt.anchorMin       = new Vector2(0.5f, 0.5f);
            spinnerRt.anchorMax       = new Vector2(0.5f, 0.5f);
            spinnerRt.pivot           = new Vector2(0.5f, 0.5f);
            spinnerRt.anchoredPosition = new Vector2(0f, 0f);
            spinnerRt.sizeDelta       = LoadingSpinnerSize;

            Image spinnerImg = spinnerGo.AddComponent<Image>();
            Sprite spinnerSprite = AssetDatabase.LoadAssetAtPath<Sprite>(LoadingSpinnerSpritePath);
            if (spinnerSprite != null)
            {
                spinnerImg.sprite = spinnerSprite;
            }
            spinnerImg.color = new Color(0.95f, 0.9f, 0.75f, 1f);

            return (enterGameButton, spinnerGo, btnText);
        }

        // ──────────────────────────────────────────────
        // 右侧功能按钮面板
        // ──────────────────────────────────────────────

        private static (BaseButton setting, BaseButton about, BaseButton announcement, BaseButton service)
            CreateRightPanel(Transform parent, TMP_FontAsset fontAsset, Sprite rectSprite)
        {
            GameObject panelGo = new GameObject("RightPanel");
            panelGo.layer = LayerMask.NameToLayer("UI");
            panelGo.transform.SetParent(parent, false);

            RectTransform rt = panelGo.AddComponent<RectTransform>();
            rt.anchorMin       = new Vector2(RightPanelAnchorX, 0.5f);
            rt.anchorMax       = new Vector2(RightPanelAnchorX, 0.5f);
            rt.pivot           = new Vector2(0.5f, 0.5f);
            rt.sizeDelta       = RightPanelSize;
            rt.anchoredPosition = new Vector2(0f, 0f);

            float halfSpacing = MenuButtonSpacing * 1.5f;
            BaseButton settingButton      = CreateMenuButton("SettingButton",      panelGo.transform, "MainEntry.BtnSetting",        new Vector2(0f,  halfSpacing), fontAsset, rectSprite);
            BaseButton aboutButton        = CreateMenuButton("AboutButton",        panelGo.transform, "MainEntry.BtnGameIntro",       new Vector2(0f,  halfSpacing - MenuButtonSpacing), fontAsset, rectSprite);
            BaseButton announcementButton = CreateMenuButton("AnnouncementButton", panelGo.transform, "MainEntry.BtnAnnouncement",    new Vector2(0f, -halfSpacing + MenuButtonSpacing), fontAsset, rectSprite);
            BaseButton serviceButton      = CreateMenuButton("ServiceButton",      panelGo.transform, "MainEntry.BtnCustomerService", new Vector2(0f, -halfSpacing), fontAsset, rectSprite);

            // 在 AnnouncementButton 上挂载 RedDot 通用红点组件
            if (announcementButton != null)
            {
                GameObject redDotPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/UI/UIForms/Common/RedDot/RedDot.prefab");
                GameObject redDotGo = (GameObject)PrefabUtility.InstantiatePrefab(redDotPrefab);
                redDotGo.name = "RedDot";
                redDotGo.transform.SetParent(announcementButton.transform, false);

                RectTransform redDotRt = redDotGo.GetComponent<RectTransform>();
                redDotRt.anchorMin = new Vector2(1f, 0f);
                redDotRt.anchorMax = new Vector2(1f, 0f);
                redDotRt.pivot = new Vector2(0.5f, 0.5f);
                redDotRt.anchoredPosition = new Vector2(-20f, 20f); // 放置在按钮右下角合适位置（略微向左上移动防裁剪）
                redDotRt.sizeDelta = new Vector2(36f, 36f); // 保持原始预制体加大尺寸

                RedDot redDotComp = redDotGo.GetComponent<RedDot>();
                if (redDotComp != null)
                {
                    SetPrivateField(redDotComp, "redDotType", Celeritas.Config.red_dot_type.announcement);
                }
                
            }

            return (settingButton, aboutButton, announcementButton, serviceButton);
        }

        // ──────────────────────────────────────────────
        // 右下角退出按钮（圆形）
        // ──────────────────────────────────────────────

        private static BaseButton CreateQuitButton(Transform parent, TMP_FontAsset fontAsset)
        {
            // 优先复用通用 CircleButton 预制体
            GameObject quitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/UI/UIForms/Common/Button/CircleButton.prefab");
            GameObject quitBtnGo;
            if (quitPrefab != null)
            {
                quitBtnGo = (GameObject)PrefabUtility.InstantiatePrefab(quitPrefab);
            }
            else
            {
                Debug.LogWarning("CircleButton prefab 未找到，手动创建 QuitButton。");
                quitBtnGo = new GameObject("QuitButton");
                quitBtnGo.layer = LayerMask.NameToLayer("UI");
                quitBtnGo.AddComponent<CircleButton>();
            }

            // 应用退出的专用图片
            Image quitImg = quitBtnGo.transform.Find("Image")?.GetComponent<Image>();
            if (quitImg == null)
            {
                quitImg = quitBtnGo.GetComponent<Image>();
                if (quitImg == null) quitImg = quitBtnGo.AddComponent<Image>();
            }
            Sprite quitSprite = AssetDatabase.LoadAssetAtPath<Sprite>(QuitSpritePath);
            if (quitSprite != null) quitImg.sprite = quitSprite;

            quitBtnGo.name = "QuitButton";
            quitBtnGo.transform.SetParent(parent, false);

            RectTransform rt = quitBtnGo.GetComponent<RectTransform>();
            rt.anchorMin       = new Vector2(1f, 0f); // 右下角锚定
            rt.anchorMax       = new Vector2(1f, 0f);
            rt.pivot           = new Vector2(1f, 0f);
            rt.anchoredPosition = new Vector2(-QuitButtonMargin, QuitButtonMargin);

            TextMeshProUGUI quitTxt = quitBtnGo.GetComponentInChildren<TextMeshProUGUI>(true);
            if (quitTxt != null)
            {
                quitTxt.text = "MainEntry.BtnQuit";
                if (fontAsset != null) quitTxt.font = fontAsset;
            }

            return quitBtnGo.GetComponent<BaseButton>();
        }

        // ──────────────────────────────────────────────
        // 底部健康忠告与版权信息面板
        // ──────────────────────────────────────────────

        private static void CreateBottomPanel(Transform parent, TMP_FontAsset fontAsset)
        {
            GameObject panelGo = new GameObject("BottomPanel");
            panelGo.layer = LayerMask.NameToLayer("UI");
            panelGo.transform.SetParent(parent, false);

            RectTransform panelRt = panelGo.AddComponent<RectTransform>();
            panelRt.anchorMin       = new Vector2(0.5f, 0f);
            panelRt.anchorMax       = new Vector2(0.5f, 0f);
            panelRt.pivot           = new Vector2(0.5f, 0f);
            panelRt.anchoredPosition = new Vector2(0f, 0f);
            panelRt.sizeDelta       = BottomPanelSize;

            // 健康游戏忠告
            GameObject adviceGo = new GameObject("HealthyAdviceText");
            adviceGo.layer = LayerMask.NameToLayer("UI");
            adviceGo.transform.SetParent(panelGo.transform, false);
            TextMeshProUGUI adviceText = adviceGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) adviceText.font = fontAsset;
            adviceText.text              = "MainEntry.HealthyGameAdvice"; // 本地化 Key
            adviceText.fontSize          = AdviceFontSize;
            adviceText.lineSpacing       = AdviceLineSpacing;
            adviceText.alignment         = TextAlignmentOptions.Center;
            adviceText.color             = new Color(0.85f, 0.8f, 0.75f, 0.9f);
            adviceText.enableWordWrapping = true;
            adviceText.overflowMode      = TextOverflowModes.Overflow;
            adviceText.raycastTarget     = false; // 纯展示，不拦截点击
            RectTransform adviceRt = adviceText.GetComponent<RectTransform>();
            adviceRt.anchorMin       = new Vector2(0.5f, 0f);
            adviceRt.anchorMax       = new Vector2(0.5f, 0f);
            adviceRt.pivot           = new Vector2(0.5f, 0f);
            adviceRt.anchoredPosition = new Vector2(0f, CopyrightBottomOffset + CopyrightTextHeight + AdviceCopyrightGap);
            adviceRt.sizeDelta       = new Vector2(900f, AdviceTextHeight);

            // 版权信息
            GameObject copyrightGo = new GameObject("CopyrightText");
            copyrightGo.layer = LayerMask.NameToLayer("UI");
            copyrightGo.transform.SetParent(panelGo.transform, false);
            TextMeshProUGUI copyrightText = copyrightGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) copyrightText.font = fontAsset;
            copyrightText.text          = "MainEntry.CopyrightText";
            copyrightText.fontSize      = CopyrightFontSize;
            copyrightText.alignment     = TextAlignmentOptions.Center;
            copyrightText.color         = new Color(0.65f, 0.6f, 0.55f, 0.8f);
            copyrightText.raycastTarget = false; // 纯展示，不拦截点击
            RectTransform copyrightRt = copyrightText.GetComponent<RectTransform>();
            copyrightRt.anchorMin       = new Vector2(0.5f, 0f);
            copyrightRt.anchorMax       = new Vector2(0.5f, 0f);
            copyrightRt.pivot           = new Vector2(0.5f, 0f);
            copyrightRt.anchoredPosition = new Vector2(0f, CopyrightBottomOffset);
            copyrightRt.sizeDelta       = new Vector2(900f, CopyrightTextHeight);
        }

        // ──────────────────────────────────────────────
        // 右上角客户端版本号
        // ──────────────────────────────────────────────

        private static TextMeshProUGUI CreateVersionText(Transform parent, TMP_FontAsset fontAsset)
        {
            GameObject go = new GameObject("VersionText");
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(parent, false);

            TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) text.font = fontAsset;
            text.text      = "v1.0.1";
            text.fontSize  = VersionFontSize;
            text.alignment = TextAlignmentOptions.Right;
            text.color     = new Color(0.9f, 0.85f, 0.75f, 0.9f);

            RectTransform rt = text.GetComponent<RectTransform>();
            rt.anchorMin       = new Vector2(1f, 1f); // 右上角锚定
            rt.anchorMax       = new Vector2(1f, 1f);
            rt.pivot           = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(-10f, -10f);
            rt.sizeDelta       = new Vector2(300f, 30f);

            return text;
        }

        // ──────────────────────────────────────────────
        // 保存 Prefab 并刷新资源库
        // ──────────────────────────────────────────────

        private static void SavePrefab(GameObject rootGo)
        {
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            if (prefabAsset != null)
            {
                Canvas prefabCanvas = prefabAsset.GetComponent<Canvas>();
                if (prefabCanvas != null)
                {
                    prefabCanvas.renderMode                  = RenderMode.ScreenSpaceCamera;
                    prefabCanvas.vertexColorAlwaysGammaSpace = true;
                    prefabCanvas.additionalShaderChannels    = AdditionalCanvasShaderChannels.TexCoord1
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

            Debug.Log($"MainEntryForm 预制体已生成：{PrefabPath}");
        }

        // ──────────────────────────────────────────────
        // 工具方法
        // ──────────────────────────────────────────────

        /// <summary>
        /// 创建右侧菜单按钮（复用 RectangleButton 预制体）。
        /// </summary>
        private static BaseButton CreateMenuButton(
            string name, Transform parent, string textKey, Vector2 anchoredPos,
            TMP_FontAsset fontAsset, Sprite rectSprite)
        {
            GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab");
            if (buttonPrefab == null)
            {
                Debug.LogError("RectangleButton prefab 未找到：Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab");
                return null;
            }

            GameObject btnGo = (GameObject)PrefabUtility.InstantiatePrefab(buttonPrefab);
            btnGo.name = name;
            btnGo.transform.SetParent(parent, false);

            RectTransform rt = btnGo.GetComponent<RectTransform>();
            rt.anchoredPosition = anchoredPos;

            TextMeshProUGUI txt = btnGo.GetComponentInChildren<TextMeshProUGUI>(true);
            if (txt != null)
            {
                txt.text = textKey;
                if (fontAsset != null) txt.font = fontAsset;
            }

            return btnGo.GetComponent<BaseButton>();
        }

        /// <summary>
        /// 通过反射向目标对象的私有字段赋值。
        /// </summary>
        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var type = obj.GetType();
            System.Reflection.FieldInfo field = null;
            while (type != null && field == null)
            {
                field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                type = type.BaseType;
            }

            if (field != null)
                field.SetValue(obj, value);
            else
                Debug.LogWarning($"字段 '{fieldName}' 在 {obj.GetType().Name} 上未找到！");
        }
    }
}
