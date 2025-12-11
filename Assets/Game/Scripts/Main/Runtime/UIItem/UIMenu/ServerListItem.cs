using System;
using Game.Scripts.Main.Runtime.Login;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UIItem.UIMenu
{
    public class ServerListItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;

        [SerializeField] private Text talentText;

        private Action<int> _onClick;
        private int _selfIndex;

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke(_selfIndex);
        }

        public void SetData(int index, LoginServerInfo loginServerInfo, Action<int> clickCallback)
        {
            _selfIndex = index;
            _onClick = clickCallback;
            talentText.text = loginServerInfo.getServerName();
            SetSelected(loginServerInfo.getServerStatus());
        }

        private void SetSelected(ServerStatusType serverStatusType)
        {
            switch (serverStatusType)
            {
                case ServerStatusType.Normal:
                {
                    imageBackground.color = Color.green;
                    break;
                }
                case ServerStatusType.Busy:
                {
                    imageBackground.color = new Color(1f, 0.5f, 0f);
                    break;
                }
                case ServerStatusType.Crowded:
                {
                    imageBackground.color = Color.yellow;
                    break;
                }
                case ServerStatusType.Full:
                {
                    imageBackground.color = Color.red;
                    break;
                }
                case ServerStatusType.Maintenance:
                {
                    imageBackground.color = Color.black;
                    break;
                }
                default:
                {
                    Log.Warning("ServerStatusType error.serverStatusType = " + serverStatusType);
                    break;
                }
            }
        }

        public override void OnRecycle()
        {
            _onClick = null;
        }
    }
}