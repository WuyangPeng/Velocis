using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class HelpItemCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Help/HelpItem.prefab";
        private const string ItemBackgroundSpritePath = "Assets/Game/Textures/Panel/Menu/help_divider_line.png";
        private const string HelpDefaultIconPath = "Assets/Game/Textures/Icon/help_default.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        private static void ConfigureTextureAsSprite(string path)
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
                
                // 程序化配置金线的九宫格边距 (左: 250px, 右: 250px) 以防止拉伸变形
                if (path.Contains("help_divider_line") && importer.spriteBorder != new Vector4(250f, 0f, 250f, 0f))
                {
                    importer.spriteBorder = new Vector4(250f, 0f, 250f, 0f);
                    changed = true;
                }

                if (changed)
                {
                    importer.SaveAndReimport();
                }
            }
        }

        [MenuItem("Generator/UI/Component/Create Help Item Prefab")]
        public static void CreateHelpItemPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Common/Help";
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "Help");
            }

            ConfigureTextureAsSprite(ItemBackgroundSpritePath);
            ConfigureTextureAsSprite(HelpDefaultIconPath);

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);
            Sprite itemBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ItemBackgroundSpritePath);
            Sprite defaultIconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(HelpDefaultIconPath);

            // 创建根节点
            GameObject rootGo = new GameObject("HelpItem", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform itemTemplateRt = rootGo.GetComponent<RectTransform>();
            itemTemplateRt.sizeDelta = new Vector2(740f, 180f);

            HelpItem helpItemComp = rootGo.AddComponent<HelpItem>();

            // 自动布局组件
            HorizontalLayoutGroup hlg = rootGo.AddComponent<HorizontalLayoutGroup>();
            hlg.padding = new RectOffset(25, 25, 20, 30);
            hlg.spacing = 25f;
            hlg.childAlignment = TextAnchor.UpperLeft;
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;

            ContentSizeFitter csfTemplate = rootGo.AddComponent<ContentSizeFitter>();
            csfTemplate.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csfTemplate.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 射线投射拦截器（透明Image），确保卡片内的空白背景区域也能响应点击与拖动事件
            Image raycastBlocker = rootGo.AddComponent<Image>();
            raycastBlocker.color = Color.clear;
            raycastBlocker.raycastTarget = true;

            // 背景 (放置于底部的装饰分割线，高度固定以防止被纵向拉伸)
            GameObject bgGo = new GameObject("Background", typeof(RectTransform));
            bgGo.layer = LayerMask.NameToLayer("UI");
            bgGo.transform.SetParent(rootGo.transform, false);
            
            RectTransform bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = new Vector2(0f, 0f); // 底部对齐
            bgRt.anchorMax = new Vector2(1f, 0f); // 左右拉伸
            bgRt.pivot = new Vector2(0.5f, 0f);
            bgRt.anchoredPosition = new Vector2(0f, 10f); // 向上偏移 10f，确保完全显示在卡片范围内不被裁剪
            bgRt.sizeDelta = new Vector2(-80f, 20f); // 左右留出 40f 边距，高度固定为 20f

            LayoutElement bgLayout = bgGo.AddComponent<LayoutElement>();
            bgLayout.ignoreLayout = true; // 忽略 LayoutGroup 自动布局控制

            Image itemBgImg = bgGo.AddComponent<Image>();
            if (itemBgSprite != null)
            {
                itemBgImg.sprite = itemBgSprite;
                itemBgImg.type = Image.Type.Sliced; // 采用九宫格渲染防止端点花纹拉伸
            }
            else
            {
                Debug.LogWarning($"HelpItemCreator: itemBgSprite not found at {ItemBackgroundSpritePath}!");
            }
            itemBgImg.color = new Color(1f, 1f, 1f, 0.8f);

            // 图标
            GameObject itemIconGo = new GameObject("Icon", typeof(RectTransform));
            itemIconGo.layer = LayerMask.NameToLayer("UI");
            itemIconGo.transform.SetParent(rootGo.transform, false);
            Image iconImg = itemIconGo.AddComponent<Image>();
            if (defaultIconSprite != null)
            {
                iconImg.sprite = defaultIconSprite;
            }
            else
            {
                Debug.LogWarning($"HelpItemCreator: defaultIconSprite not found at {HelpDefaultIconPath}!");
            }
            LayoutElement iconLayout = itemIconGo.AddComponent<LayoutElement>();
            iconLayout.minWidth = 120f;
            iconLayout.minHeight = 120f;
            iconLayout.preferredWidth = 120f;
            iconLayout.preferredHeight = 120f;

            // 文本容器
            GameObject textContainerGo = new GameObject("TextContainer", typeof(RectTransform));
            textContainerGo.layer = LayerMask.NameToLayer("UI");
            textContainerGo.transform.SetParent(rootGo.transform, false);
            LayoutElement containerLayout = textContainerGo.AddComponent<LayoutElement>();
            containerLayout.flexibleWidth = 1f;

            VerticalLayoutGroup vlg = textContainerGo.AddComponent<VerticalLayoutGroup>();
            vlg.spacing = 10f;
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;

            ContentSizeFitter csfText = textContainerGo.AddComponent<ContentSizeFitter>();
            csfText.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csfText.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 标题
            GameObject itemTitleGo = new GameObject("Title", typeof(RectTransform));
            itemTitleGo.layer = LayerMask.NameToLayer("UI");
            itemTitleGo.transform.SetParent(textContainerGo.transform, false);
            TextMeshProUGUI itemTitleTxt = itemTitleGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) itemTitleTxt.font = fontAsset;
            itemTitleTxt.fontSize = 26f;
            itemTitleTxt.color = new Color(0.95f, 0.85f, 0.6f, 1f);

            // 描述
            GameObject itemDescGo = new GameObject("Description", typeof(RectTransform));
            itemDescGo.layer = LayerMask.NameToLayer("UI");
            itemDescGo.transform.SetParent(textContainerGo.transform, false);
            TextMeshProUGUI itemDescTxt = itemDescGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null) itemDescTxt.font = fontAsset;
            itemDescTxt.fontSize = 22f;
            itemDescTxt.color = new Color(1f, 0.95f, 0.85f, 1f);
            itemDescTxt.enableWordWrapping = true;

            // 设置 HelpItem 字段绑定 via reflection
            SetPrivateField(helpItemComp, "titleText", itemTitleTxt);
            SetPrivateField(helpItemComp, "descriptionText", itemDescTxt);
            SetPrivateField(helpItemComp, "iconImage", iconImg);

            // 保存 Prefab
            SavePrefab(rootGo);
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
            Debug.LogError($"[HelpItemCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }

        private static void SavePrefab(GameObject rootGo)
        {
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"HelpItem Prefab successfully generated at: {PrefabPath}");
        }
    }
}
