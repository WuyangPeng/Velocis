using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    /// <summary>
    /// 公告列表左侧条目组件。由 AnnouncementItemCreator 反射绑定序列化字段。
    /// </summary>
    public class AnnouncementItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text tagText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private BaseButton button;

        public TMP_Text TitleText => titleText;
        public TMP_Text TagText => tagText;
        public Image BackgroundImage => backgroundImage;
        public BaseButton Button => button;
    }
}
