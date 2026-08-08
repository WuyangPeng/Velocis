using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class AnnouncementItemCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Announcement/AnnouncementItem.prefab";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";
        private const string NormalSpritePath = "Assets/Game/Textures/Button/Menu/category_btn_normal.png";
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";

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

        [MenuItem("Generator/UI/Component/Create Announcement Item Prefab")]
        public static void CreateAnnouncementItemPrefab()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common/Announcement"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "Announcement");
            }

            ConfigureTextureAsSprite(NormalSpritePath);

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            var normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(NormalSpritePath);
            var rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);

            if (rectButtonPrefab == null)
            {
                Debug.LogError($"RectangleButtonPrefab not found at {RectangleButtonPrefabPath}");
                return;
            }

            var rootGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
            rootGo.name = "AnnouncementItem";
            var baseButton = rootGo.GetComponent<BaseButton>();
            var backgroundImage = rootGo.transform.Find("Image")?.GetComponent<Image>();
            if (backgroundImage != null && normalSprite != null)
            {
                backgroundImage.sprite = normalSprite;
                backgroundImage.type = Image.Type.Sliced;
            }

            // 获取并修改预制体自带的文字组件，将其作为Title
            var titleTxt = rootGo.transform.Find("Text").GetComponent<TMP_Text>();
            titleTxt.gameObject.name = "Title";

            var rootRt = rootGo.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(260f, 70f);

            UIEditorCreatorUtility.SetPrivateField(baseButton, "clickSoundId", 100016);

            var itemComp = rootGo.GetComponent<AnnouncementItem>();
            if (itemComp == null)
            {
                itemComp = rootGo.AddComponent<AnnouncementItem>();
            }

            // Tag (左侧文字)
            var tagGo = new GameObject("Tag", typeof(RectTransform));
            tagGo.layer = LayerMask.NameToLayer("UI");
            tagGo.transform.SetParent(rootGo.transform, false);
            var tagRt = tagGo.GetComponent<RectTransform>();
            tagRt.anchorMin = new Vector2(0f, 0.5f);
            tagRt.anchorMax = new Vector2(0f, 0.5f);
            tagRt.pivot = new Vector2(0f, 0.5f);
            tagRt.anchoredPosition = new Vector2(25f, 0f);
            tagRt.sizeDelta = new Vector2(36f, 36f);
            var tagTxt = tagGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                tagTxt.font = fontAsset;
            }

            tagTxt.fontSize = 20f;
            tagTxt.alignment = TextAlignmentOptions.Center;
            tagTxt.color = new Color(0.85f, 0.2f, 0.15f, 1f);
            tagTxt.text = string.Empty;
            tagTxt.raycastTarget = false;

            // Title (原本的文字，向右移动并左对齐)
            var titleRt = titleTxt.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0f, 0f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.offsetMin = new Vector2(70f, 8f);
            titleRt.offsetMax = new Vector2(-12f, -8f);
            if (fontAsset != null)
            {
                titleTxt.font = fontAsset;
            }

            titleTxt.fontSize = 22f;
            titleTxt.alignment = TextAlignmentOptions.Left;
            titleTxt.color = Color.white;
            titleTxt.enableWordWrapping = false;
            titleTxt.overflowMode = TextOverflowModes.Ellipsis;
            titleTxt.raycastTarget = false;
            titleTxt.text = "公告标题";

            UIEditorCreatorUtility.SetPrivateField(itemComp, "titleText", titleTxt);
            UIEditorCreatorUtility.SetPrivateField(itemComp, "tagText", tagTxt);
            UIEditorCreatorUtility.SetPrivateField(itemComp, "backgroundImage", backgroundImage);
            UIEditorCreatorUtility.SetPrivateField(itemComp, "button", baseButton);

            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"AnnouncementItem Prefab successfully generated at: {PrefabPath}");
        }
    }
}
