// 创建时间：2026-07-27
// 修改时间：2026-07-27

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class ServerListItemCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/ServerList/ServerListItem.prefab";
        private const string BackgroundSpritePath = "Assets/Game/Textures/Button/Menu/server_card_bg.png";
        private const string StatusDotSpritePath = "Assets/Game/Textures/Button/Menu/status_dot_icon.png";
        private const string CharacterStampSpritePath = "Assets/Game/Textures/Button/Menu/stamp_character.png";
        private const string SelectFrameSpritePath = "Assets/Game/Textures/Button/Menu/select_frame.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";

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

        [MenuItem("Generator/UI/Component/Create Server List Item Prefab")]
        public static void CreateServerListItemPrefab()
        {
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }
            const string serverListFolderPath = "Assets/Game/UI/UIForms/Common/ServerList";
            if (!AssetDatabase.IsValidFolder(serverListFolderPath))
            {
                AssetDatabase.CreateFolder(folderPath, "ServerList");
            }

            ConfigureTextureAsSprite(BackgroundSpritePath);
            ConfigureTextureAsSprite(StatusDotSpritePath);
            ConfigureTextureAsSprite(CharacterStampSpritePath);
            ConfigureTextureAsSprite(SelectFrameSpritePath);

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackgroundSpritePath);
            Sprite dotSprite = AssetDatabase.LoadAssetAtPath<Sprite>(StatusDotSpritePath);
            Sprite stampSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CharacterStampSpritePath);
            Sprite selectFrameSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SelectFrameSpritePath);
            GameObject rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);

            GameObject rootGo = new GameObject("ServerListItem", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rootRt = rootGo.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(340f, 130f);

            ServerListItem comp = rootGo.AddComponent<ServerListItem>();

            // 1. Background
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
                bgImg.type = Image.Type.Sliced;
            }
            bgImg.color = Color.white;
            bgImg.raycastTarget = false;

            // 2. Click Button (as raycast target cover)
            BaseButton buttonComp = null;
            if (rectButtonPrefab != null)
            {
                GameObject btnGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                btnGo.name = "SelectButton";
                btnGo.transform.SetParent(rootGo.transform, false);
                RectTransform btnRt = btnGo.GetComponent<RectTransform>();
                btnRt.anchorMin = Vector2.zero;
                btnRt.anchorMax = Vector2.one;
                btnRt.sizeDelta = Vector2.zero;
                btnRt.anchoredPosition = Vector2.zero;

                // Make visual representation of the child button invisible/transparent, but keep raycast target active
                var btnImg = btnGo.transform.Find("Image")?.GetComponent<Image>();
                if (btnImg != null)
                {
                    btnImg.color = Color.clear;
                }
                var btnTxt = btnGo.GetComponentInChildren<TMP_Text>();
                if (btnTxt != null)
                {
                    btnTxt.text = string.Empty;
                }

                buttonComp = btnGo.GetComponent<BaseButton>();
                if (buttonComp != null)
                {
                    SetPrivateField(buttonComp, "clickSoundId", 100023);
                }
            }

            // 3. Status Dot Indicator
            GameObject dotGo = new GameObject("StatusIndicator", typeof(RectTransform));
            dotGo.layer = LayerMask.NameToLayer("UI");
            dotGo.transform.SetParent(rootGo.transform, false);
            RectTransform dotRt = dotGo.GetComponent<RectTransform>();
            dotRt.anchorMin = new Vector2(0f, 0.5f);
            dotRt.anchorMax = new Vector2(0f, 0.5f);
            dotRt.pivot = new Vector2(0.5f, 0.5f);
            dotRt.anchoredPosition = new Vector2(68f, 15f);
            dotRt.sizeDelta = new Vector2(20f, 20f);
            Image dotImg = dotGo.AddComponent<Image>();
            if (dotSprite != null)
            {
                dotImg.sprite = dotSprite;
            }
            dotImg.raycastTarget = false;

            // 4. Server Name
            GameObject nameGo = new GameObject("ServerNameText", typeof(RectTransform));
            nameGo.layer = LayerMask.NameToLayer("UI");
            nameGo.transform.SetParent(rootGo.transform, false);
            RectTransform nameRt = nameGo.GetComponent<RectTransform>();
            nameRt.anchorMin = new Vector2(0f, 0.5f);
            nameRt.anchorMax = new Vector2(1f, 0.5f);
            nameRt.pivot = new Vector2(0f, 0.5f);
            nameRt.anchoredPosition = new Vector2(88f, 15f);
            nameRt.sizeDelta = new Vector2(-190f, 32f);
            TextMeshProUGUI nameTxt = nameGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) nameTxt.font = fontAsset;
            nameTxt.text = "幽州逐鹿";
            nameTxt.fontSize = 22f;
            nameTxt.alignment = TextAlignmentOptions.Left;
            nameTxt.color = new Color(0.95f, 0.85f, 0.6f, 1f);
            nameTxt.enableWordWrapping = false;
            nameTxt.overflowMode = TextOverflowModes.Ellipsis;
            nameTxt.raycastTarget = false;

            // 5. Player Role Name / Status Text
            GameObject playerGo = new GameObject("PlayerNameText", typeof(RectTransform));
            playerGo.layer = LayerMask.NameToLayer("UI");
            playerGo.transform.SetParent(rootGo.transform, false);
            RectTransform playerRt = playerGo.GetComponent<RectTransform>();
            playerRt.anchorMin = new Vector2(0f, 0.5f);
            playerRt.anchorMax = new Vector2(1f, 0.5f);
            playerRt.pivot = new Vector2(0f, 0.5f);
            playerRt.anchoredPosition = new Vector2(88f, -18f);
            playerRt.sizeDelta = new Vector2(-190f, 28f);
            TextMeshProUGUI playerTxt = playerGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) playerTxt.font = fontAsset;
            playerTxt.text = "";
            playerTxt.fontSize = 17f;
            playerTxt.alignment = TextAlignmentOptions.Left;
            playerTxt.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            playerTxt.enableWordWrapping = false;
            playerTxt.overflowMode = TextOverflowModes.Ellipsis;
            playerTxt.raycastTarget = false;

            // 6. Ping Text
            GameObject pingGo = new GameObject("PingText", typeof(RectTransform));
            pingGo.layer = LayerMask.NameToLayer("UI");
            pingGo.transform.SetParent(rootGo.transform, false);
            RectTransform pingRt = pingGo.GetComponent<RectTransform>();
            pingRt.anchorMin = new Vector2(1f, 0.5f);
            pingRt.anchorMax = new Vector2(1f, 0.5f);
            pingRt.pivot = new Vector2(1f, 0.5f);
            pingRt.anchoredPosition = new Vector2(-68f, 15f);
            pingRt.sizeDelta = new Vector2(90f, 28f);
            TextMeshProUGUI pingTxt = pingGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) pingTxt.font = fontAsset;
            pingTxt.text = "30ms";
            pingTxt.fontSize = 16f;
            pingTxt.alignment = TextAlignmentOptions.Right;
            pingTxt.color = Color.green;
            pingTxt.raycastTarget = false;

            // 7. Character Mark Stamp
            GameObject stampGo = new GameObject("CharacterMark", typeof(RectTransform));
            stampGo.layer = LayerMask.NameToLayer("UI");
            stampGo.transform.SetParent(rootGo.transform, false);
            RectTransform stampRt = stampGo.GetComponent<RectTransform>();
            stampRt.anchorMin = new Vector2(1f, 0.5f);
            stampRt.anchorMax = new Vector2(1f, 0.5f);
            stampRt.pivot = new Vector2(1f, 0.5f);
            stampRt.anchoredPosition = new Vector2(-68f, -18f);
            stampRt.sizeDelta = new Vector2(32f, 32f);
            Image stampImg = stampGo.AddComponent<Image>();
            if (stampSprite != null)
            {
                stampImg.sprite = stampSprite;
            }
            stampImg.raycastTarget = false;
            stampGo.SetActive(false);

            // 8. Select Frame (glowing golden outline)
            GameObject selectGo = new GameObject("SelectFrame", typeof(RectTransform));
            selectGo.layer = LayerMask.NameToLayer("UI");
            selectGo.transform.SetParent(rootGo.transform, false);
            RectTransform selectRt = selectGo.GetComponent<RectTransform>();
            selectRt.anchorMin = Vector2.zero;
            selectRt.anchorMax = Vector2.one;
            selectRt.sizeDelta = Vector2.zero;
            Image selectImg = selectGo.AddComponent<Image>();
            if (selectFrameSprite != null)
            {
                selectImg.sprite = selectFrameSprite;
                selectImg.type = Image.Type.Sliced;
                selectImg.pixelsPerUnitMultiplier = 2f;
            }
            selectImg.raycastTarget = false;
            selectGo.SetActive(false);

            // Bind Fields
            SetPrivateField(comp, "imageBackground", bgImg);
            SetPrivateField(comp, "serverNameText", nameTxt);
            SetPrivateField(comp, "playerNameText", playerTxt);
            SetPrivateField(comp, "statusIndicator", dotImg);
            SetPrivateField(comp, "pingText", pingTxt);
            SetPrivateField(comp, "characterMark", stampGo);
            SetPrivateField(comp, "button", buttonComp);
            SetPrivateField(comp, "selectFrame", selectGo);

            // Save Prefab
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"Successfully generated ServerListItem prefab at: {PrefabPath}");
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
            Debug.LogError($"[ServerListItemCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }
    }
}
