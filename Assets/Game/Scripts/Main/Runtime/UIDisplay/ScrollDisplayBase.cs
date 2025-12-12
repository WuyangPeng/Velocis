using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIDisplay
{
    public class ScrollDisplayBase : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private Transform content;

        protected GameObject GetRowGameObject(int row, TextAnchor childAlignment = TextAnchor.LowerLeft,
            int vertical = 100)
        {
            var rowGameObject = new GameObject($"Row{row}", typeof(RectTransform));
            rowGameObject.transform.SetParent(content, false);

            var horizontalLayoutGroup = rowGameObject.AddComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.spacing = 20f;
            horizontalLayoutGroup.childControlWidth = false;
            horizontalLayoutGroup.childControlHeight = false;
            horizontalLayoutGroup.childAlignment = childAlignment;
            horizontalLayoutGroup.childForceExpandWidth = false;

            var rowRectTransform = rowGameObject.GetComponent<RectTransform>();
            rowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
            rowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vertical);

            return rowGameObject;
        }
    }
}