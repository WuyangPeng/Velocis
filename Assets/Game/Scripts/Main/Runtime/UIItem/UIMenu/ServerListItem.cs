using System;
using Game.Scripts.Main.Runtime.Login;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UIItem.UIMenu
{
    public class ServerListItem : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imageBackground;

        [SerializeField] private TMP_Text serverNameText;

        [SerializeField] private TMP_Text playerNameText;

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
            serverNameText.text = loginServerInfo.server_name;
            playerNameText.text = loginServerInfo.getPlayerName();
            SetSelected(loginServerInfo.server_status);
        }

        private void SetSelected(ServerStatusType serverStatusType)
        {
            switch (serverStatusType)
            {
                case ServerStatusType.Normal:
                {
                    serverNameText.color = new Color(0.6f, 0.98f, 0.6f, 0.9f);
                    break;
                }
                case ServerStatusType.Busy:
                {
                    serverNameText.color = new Color(1f, 0.65f, 0.31f, 0.9f);
                    break;
                }
                case ServerStatusType.Crowded:
                {
                    serverNameText.color = new Color(0.96f, 0.82f, 0.25f, 0.9f);
                    break;
                }
                case ServerStatusType.Full:
                {
                    serverNameText.color = new Color(0.8f, 0.36f, 0.36f, 0.9f);
                    break;
                }
                case ServerStatusType.Maintenance:
                {
                    serverNameText.color = new Color(0.74f, 0.76f, 0.78f, 0.9f);
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