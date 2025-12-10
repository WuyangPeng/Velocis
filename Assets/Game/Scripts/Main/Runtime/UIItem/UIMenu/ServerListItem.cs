using System;
using Game.Scripts.Main.Runtime.DataTable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIItem.UIMenu
{
    public class ServerListItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;

        [SerializeField] private Text talentText;

        private Action<int> onClick;
        private int selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(selfIndex);
        }

        public void SetData(int index, DRTalent data, Action<int> clickCallback)
        {
            selfIndex = index;
            onClick = clickCallback;
            talentText.text = GameEntry.Localization.GetString(data.Name);
        }

        public void SetSelected(bool selected)
        {
            imageBackground.color = selected ? Color.blue : Color.yellow;
        }

        public override void OnRecycle()
        {
            onClick = null;
        }
    }
}