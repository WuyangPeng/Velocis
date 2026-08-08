using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class CategoryButtonGroup : MonoBehaviour
    {
        [SerializeField] private RectTransform categoryContainer;
        [SerializeField] private GameObject categoryButtonTemplate;
        [SerializeField] private Sprite categoryNormalSprite;
        [SerializeField] private Sprite categorySelectedSprite;
        [SerializeField] [UISoundId] private int tabSwitchSoundId;

        public RectTransform CategoryContainer => categoryContainer;
        public GameObject CategoryButtonTemplate => categoryButtonTemplate;
        public Sprite CategoryNormalSprite => categoryNormalSprite;
        public Sprite CategorySelectedSprite => categorySelectedSprite;
        public int TabSwitchSoundId => tabSwitchSoundId;

        public void PlayTabSwitchSound()
        {
            if (tabSwitchSoundId > 0)
            {
                GameEntry.Sound.PlayUISound(tabSwitchSoundId);
            }
        }
    }
}