using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class UpdateResourceForm : MonoBehaviour
    {
        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Slider progressSlider;

        public void SetProgress(float progress, string description)
        {
            progressSlider.value = progress;
            descriptionText.text = description;
        }
    }
}
