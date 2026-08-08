// 创建时间：2026-07-24
// 修改时间：2026-07-24

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    /// <summary>
    /// 公告详情面板组件。由 AnnouncementDetailCreator 反射绑定序列化字段。
    /// </summary>
    public class AnnouncementDetail : MonoBehaviour
    {
        [SerializeField] private RawImage bannerImage;
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private RectTransform detailContent;

        public RawImage BannerImage => bannerImage;
        public TMP_Text ContentText => contentText;
        public RectTransform DetailContent => detailContent;
    }
}
