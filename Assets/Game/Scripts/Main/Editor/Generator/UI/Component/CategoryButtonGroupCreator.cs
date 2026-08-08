using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class CategoryButtonGroupCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Control/CategoryButtonGroup.prefab";
        private const string RectangleButtonPrefabPath = "Assets/Game/UI/UIForms/Common/Button/RectangleButton.prefab";
        private const string CategoryBtnNormalPath = "Assets/Game/Textures/Button/Menu/category_btn_normal.png";
        private const string CategoryBtnSelectedPath = "Assets/Game/Textures/Button/Menu/category_btn_selected.png";

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

        [MenuItem("Generator/UI/Component/Create Category Button Group Prefab")]
        public static void CreateCategoryButtonGroupPrefab()
        {
            // 确保目录存在
            const string folderPath = "Assets/Game/UI/UIForms/Common";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            ConfigureTextureAsSprite(CategoryBtnNormalPath);
            ConfigureTextureAsSprite(CategoryBtnSelectedPath);

            Sprite categoryNormalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CategoryBtnNormalPath);
            Sprite categorySelectedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(CategoryBtnSelectedPath);
            GameObject rectButtonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RectangleButtonPrefabPath);

            // 创建根节点
            GameObject rootGo = new GameObject("CategoryButtonGroup", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rootRt = rootGo.GetComponent<RectTransform>();
            rootRt.anchorMin = new Vector2(0f, 0.5f);
            rootRt.anchorMax = new Vector2(0f, 0.5f);
            rootRt.pivot = new Vector2(0f, 0.5f);
            rootRt.anchoredPosition = new Vector2(60f, -40f);
            rootRt.sizeDelta = new Vector2(280f, 600f);

            CategoryButtonGroup groupComp = rootGo.AddComponent<CategoryButtonGroup>();

            // Category Scroll View
            GameObject scrollCategoryGo = new GameObject("CategoryScrollView", typeof(RectTransform));
            scrollCategoryGo.layer = LayerMask.NameToLayer("UI");
            scrollCategoryGo.transform.SetParent(rootGo.transform, false);
            RectTransform scrollCategoryRt = scrollCategoryGo.GetComponent<RectTransform>();
            scrollCategoryRt.anchorMin = Vector2.zero;
            scrollCategoryRt.anchorMax = Vector2.one;
            scrollCategoryRt.sizeDelta = Vector2.zero;

            ScrollRect categoryScroll = scrollCategoryGo.AddComponent<ScrollRect>();
            categoryScroll.horizontal = false;
            categoryScroll.vertical = true;
            categoryScroll.scrollSensitivity = 25f;

            // Viewport
            GameObject viewportCatGo = new GameObject("Viewport", typeof(RectTransform));
            viewportCatGo.layer = LayerMask.NameToLayer("UI");
            viewportCatGo.transform.SetParent(scrollCategoryGo.transform, false);
            RectTransform viewportCatRt = viewportCatGo.GetComponent<RectTransform>();
            viewportCatRt.anchorMin = Vector2.zero;
            viewportCatRt.anchorMax = Vector2.one;
            viewportCatRt.sizeDelta = Vector2.zero;
            viewportCatGo.AddComponent<RectMask2D>();
            categoryScroll.viewport = viewportCatRt;

            // Content Container
            GameObject containerCatGo = new GameObject("Content", typeof(RectTransform));
            containerCatGo.layer = LayerMask.NameToLayer("UI");
            containerCatGo.transform.SetParent(viewportCatGo.transform, false);
            RectTransform containerCatRt = containerCatGo.GetComponent<RectTransform>();
            containerCatRt.anchorMin = new Vector2(0f, 1f);
            containerCatRt.anchorMax = new Vector2(1f, 1f);
            containerCatRt.pivot = new Vector2(0.5f, 1f);
            containerCatRt.sizeDelta = new Vector2(0f, 0f);

            VerticalLayoutGroup vlgCat = containerCatGo.AddComponent<VerticalLayoutGroup>();
            vlgCat.spacing = 10f;
            vlgCat.childAlignment = TextAnchor.UpperCenter;
            vlgCat.childControlHeight = false;
            vlgCat.childControlWidth = false;
            vlgCat.childForceExpandHeight = false;
            vlgCat.childForceExpandWidth = false;

            ContentSizeFitter csfCat = containerCatGo.AddComponent<ContentSizeFitter>();
            csfCat.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            categoryScroll.content = containerCatRt;

            // Template Category Button
            GameObject catTemplateGo = null;
            if (rectButtonPrefab != null)
            {
                catTemplateGo = (GameObject)PrefabUtility.InstantiatePrefab(rectButtonPrefab);
                catTemplateGo.name = "CategoryButtonTemplate";
                catTemplateGo.transform.SetParent(containerCatGo.transform, false);
                RectTransform templateRt = catTemplateGo.GetComponent<RectTransform>();
                templateRt.sizeDelta = new Vector2(260f, 65f);

                // Add SelectFrame for select outline visual state
                GameObject selectFrameGo = new GameObject("SelectFrame", typeof(RectTransform));
                selectFrameGo.layer = LayerMask.NameToLayer("UI");
                selectFrameGo.transform.SetParent(catTemplateGo.transform, false);
                RectTransform sfRt = selectFrameGo.GetComponent<RectTransform>();
                sfRt.anchorMin = Vector2.zero;
                sfRt.anchorMax = Vector2.one;
                sfRt.sizeDelta = Vector2.zero;
                Image sfImg = selectFrameGo.AddComponent<Image>();
                sfImg.color = new Color(0.95f, 0.85f, 0.6f, 0.4f);
                selectFrameGo.SetActive(false);
            }
            else
            {
                Debug.LogError($"[CategoryButtonGroupCreator] Cannot load RectangleButton prefab at: {RectangleButtonPrefabPath}");
            }

            // Set CategoryButtonGroup fields via reflection
            SetPrivateField(groupComp, "categoryContainer", containerCatRt);
            SetPrivateField(groupComp, "categoryButtonTemplate", catTemplateGo);
            SetPrivateField(groupComp, "categoryNormalSprite", categoryNormalSprite);
            SetPrivateField(groupComp, "categorySelectedSprite", categorySelectedSprite);
            SetPrivateField(groupComp, "tabSwitchSoundId", 100014);

            // Save Prefab
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
            Debug.LogError($"[CategoryButtonGroupCreator] Field '{fieldName}' not found on hierarchy of {target.GetType().Name}");
        }

        private static void SavePrefab(GameObject rootGo)
        {
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"CategoryButtonGroup Prefab successfully generated at: {PrefabPath}");
        }
    }
}
