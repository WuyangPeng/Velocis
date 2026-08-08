// 创建时间：2026-07-23
// 修改时间：2026-07-23
// 审核时间：

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class RedDotCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/RedDot/RedDot.prefab";
        private const string SpritePath = "Assets/Game/Textures/Button/Menu/red_dot_icon.png";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        [MenuItem("Generator/UI/Component/Create Red Dot Prefab")]
        public static void CreateRedDotPrefab()
        {
            // 确保目录存在 Assets/Game/UI/UIForms/Common/RedDot
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms"))
                {
                    AssetDatabase.CreateFolder("Assets/Game/UI", "UIForms");
                }
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common/RedDot"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "RedDot");
            }

            Sprite dotSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath);
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);

            // 根节点
            GameObject rootGo = new GameObject("RedDot", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            RectTransform rt = rootGo.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(36f, 36f);

            // 红点背景图片组件
            Image img = rootGo.AddComponent<Image>();
            img.sprite = dotSprite;
            img.raycastTarget = false;

            // 数字文本子节点
            GameObject countGo = new GameObject("CountText", typeof(RectTransform));
            countGo.layer = LayerMask.NameToLayer("UI");
            countGo.transform.SetParent(rootGo.transform, false);

            RectTransform countRt = countGo.GetComponent<RectTransform>();
            countRt.anchorMin = Vector2.zero;
            countRt.anchorMax = Vector2.one;
            countRt.offsetMin = Vector2.zero;
            countRt.offsetMax = Vector2.zero;

            TextMeshProUGUI countTxt = countGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                countTxt.font = fontAsset;
            }
            countTxt.fontSize = 15f;
            countTxt.fontStyle = FontStyles.Bold;
            countTxt.alignment = TextAlignmentOptions.Center;
            countTxt.color = new Color(1f, 0.94f, 0.75f, 1f); // 暖金色/米色文本，契合古风红点描边底图
            countTxt.raycastTarget = false;
            countTxt.text = string.Empty;

            // 添加 RedDot 逻辑组件并反射绑定序列化私有字段
            RedDot redDotComp = rootGo.AddComponent<RedDot>();
            UIEditorCreatorUtility.SetPrivateField(redDotComp, "redDotImage", img);
            UIEditorCreatorUtility.SetPrivateField(redDotComp, "countText", countTxt);

            // 保存 Prefab
            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"RedDot Prefab successfully generated at: {PrefabPath}");
        }
    }
}
