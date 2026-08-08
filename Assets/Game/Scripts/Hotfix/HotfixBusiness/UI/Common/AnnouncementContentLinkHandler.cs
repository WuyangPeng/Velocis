using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    /// <summary>
    /// 公告正文超链接点击处理：点击 TMP link 时外调系统浏览器。
    /// </summary>
    public class AnnouncementContentLinkHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text contentText;

        public void Bind(TMP_Text text)
        {
            contentText = text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (contentText == null)
            {
                return;
            }

            Camera eventCamera = null;
            var canvas = contentText.canvas;
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                eventCamera = canvas.worldCamera;
            }

            var linkIndex = TMP_TextUtilities.FindIntersectingLink(contentText, eventData.position, eventCamera);
            if (linkIndex == -1)
            {
                return;
            }

            var linkInfo = contentText.textInfo.linkInfo[linkIndex];
            var linkId = linkInfo.GetLinkID();
            if (!string.IsNullOrEmpty(linkId))
            {
                Application.OpenURL(linkId);
            }
        }
    }
}
