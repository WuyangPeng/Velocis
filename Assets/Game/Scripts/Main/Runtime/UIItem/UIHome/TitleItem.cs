using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Main.Runtime.UIItem.UIHome
{
    public class TitleItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private TMP_Text textTitle;

        private Action<int> _onClick;
        private int _selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_selfIndex);
        }

        /*  public void SetData(int index, title_config data, Action<int> clickCallback)
          {
              _selfIndex = index;
              _onClick = clickCallback;

              if (textTitle != null)
              {
                  textTitle.text = data.Text;
              }
          }*/

        public void SetSelected(bool selected)
        {
            imageBackground.color = selected ? Color.yellow : Color.white;
        }

        public void SetGrayscale(bool isGrayscale)
        {
            if (textTitle != null)
            {
                textTitle.color = isGrayscale ? Color.gray : Color.white;
            }
        }

        public override void OnRecycle()
        {
            if (textTitle != null)
            {
                textTitle.text = string.Empty;
                textTitle.color = Color.white;
            }

            _onClick = null;
        }
    }
}