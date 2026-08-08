// 创建时间：2026-07-24
// 修改时间：2026-07-24

using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Editor.Generator.UI.Component
{
    public static class AnnouncementDetailCreator
    {
        public const string PrefabPath = "Assets/Game/UI/UIForms/Common/Announcement/AnnouncementDetail.prefab";
        private const string FontPath = "Assets/Game/Fonts/NotoSerifSC-Black SDF.asset";

        [MenuItem("Generator/UI/Component/Create Announcement Detail Prefab")]
        public static void CreateAnnouncementDetailPrefab()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms", "Common");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Game/UI/UIForms/Common/Announcement"))
            {
                AssetDatabase.CreateFolder("Assets/Game/UI/UIForms/Common", "Announcement");
            }

            var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FontPath);

            var rootGo = new GameObject("AnnouncementDetail", typeof(RectTransform));
            rootGo.layer = LayerMask.NameToLayer("UI");
            var rootRt = rootGo.GetComponent<RectTransform>();
            rootRt.sizeDelta = new Vector2(570f, 500f);

            var detailScrollGo = new GameObject("DetailScrollView");
            detailScrollGo.layer = LayerMask.NameToLayer("UI");
            detailScrollGo.transform.SetParent(rootGo.transform, false);
            var detailScrollRt = detailScrollGo.AddComponent<RectTransform>();
            detailScrollRt.anchorMin = Vector2.zero;
            detailScrollRt.anchorMax = Vector2.one;
            detailScrollRt.sizeDelta = Vector2.zero;
            var detailScroll = detailScrollGo.AddComponent<ScrollRect>();
            detailScroll.horizontal = false;
            detailScroll.vertical = true;
            detailScroll.scrollSensitivity = 25f;

            var detailViewportGo = new GameObject("Viewport");
            detailViewportGo.layer = LayerMask.NameToLayer("UI");
            detailViewportGo.transform.SetParent(detailScrollGo.transform, false);
            var detailViewportRt = detailViewportGo.AddComponent<RectTransform>();
            detailViewportRt.anchorMin = Vector2.zero;
            detailViewportRt.anchorMax = Vector2.one;
            detailViewportRt.sizeDelta = Vector2.zero;
            var detailViewportImg = detailViewportGo.AddComponent<Image>();
            detailViewportImg.color = new Color(0f, 0f, 0f, 0f);
            detailViewportImg.raycastTarget = true;
            detailViewportGo.AddComponent<RectMask2D>();
            detailScroll.viewport = detailViewportRt;

            var detailContentGo = new GameObject("Content");
            detailContentGo.layer = LayerMask.NameToLayer("UI");
            detailContentGo.transform.SetParent(detailViewportGo.transform, false);
            var detailContentRt = detailContentGo.AddComponent<RectTransform>();
            detailContentRt.anchorMin = new Vector2(0f, 1f);
            detailContentRt.anchorMax = new Vector2(1f, 1f);
            detailContentRt.pivot = new Vector2(0.5f, 1f);
            detailContentRt.sizeDelta = new Vector2(0f, 0f);
            var detailVlg = detailContentGo.AddComponent<VerticalLayoutGroup>();
            detailVlg.spacing = 16f;
            detailVlg.padding = new RectOffset(8, 8, 8, 8);
            detailVlg.childAlignment = TextAnchor.UpperCenter;
            detailVlg.childControlHeight = true;
            detailVlg.childControlWidth = true;
            detailVlg.childForceExpandHeight = false;
            detailVlg.childForceExpandWidth = true;
            var detailCsf = detailContentGo.AddComponent<ContentSizeFitter>();
            detailCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            detailScroll.content = detailContentRt;

            // Banner
            var bannerGo = new GameObject("Banner");
            bannerGo.layer = LayerMask.NameToLayer("UI");
            bannerGo.transform.SetParent(detailContentGo.transform, false);
            var bannerRaw = bannerGo.AddComponent<RawImage>();
            bannerRaw.color = Color.white;
            var bannerLayout = bannerGo.AddComponent<LayoutElement>();
            bannerLayout.preferredHeight = 180f;
            bannerLayout.minHeight = 180f;
            bannerGo.SetActive(false);

            // Content text
            var contentGo = new GameObject("ContentText");
            contentGo.layer = LayerMask.NameToLayer("UI");
            contentGo.transform.SetParent(detailContentGo.transform, false);
            var contentTxt = contentGo.AddComponent<TextMeshProUGUI>();
            if (fontAsset != null)
            {
                contentTxt.font = fontAsset;
            }

            contentTxt.fontSize = 22f;
            contentTxt.color = new Color(1f, 0.95f, 0.85f, 1f);
            contentTxt.enableWordWrapping = true;
            contentTxt.richText = true;
            contentTxt.raycastTarget = true;
            contentTxt.alignment = TextAlignmentOptions.TopLeft;
            var contentCsf = contentGo.AddComponent<ContentSizeFitter>();
            contentCsf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentGo.AddComponent<AnnouncementContentLinkHandler>();

            var detailComp = rootGo.AddComponent<AnnouncementDetail>();
            UIEditorCreatorUtility.SetPrivateField(detailComp, "bannerImage", bannerRaw);
            UIEditorCreatorUtility.SetPrivateField(detailComp, "contentText", contentTxt);
            UIEditorCreatorUtility.SetPrivateField(detailComp, "detailContent", detailContentRt);

            PrefabUtility.SaveAsPrefabAsset(rootGo, PrefabPath);
            Object.DestroyImmediate(rootGo);
            Debug.Log($"AnnouncementDetail Prefab successfully generated at: {PrefabPath}");
        }
    }
}
