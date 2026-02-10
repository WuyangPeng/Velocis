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
                    serverNameText.color = Color.green;
                    break;
                }
                case ServerStatusType.Busy:
                {
                    serverNameText.color = new Color(1f, 0.5f, 0f);
                    break;
                }
                case ServerStatusType.Crowded:
                {
                    serverNameText.color = Color.yellow;
                    break;
                }
                case ServerStatusType.Full:
                {
                    serverNameText.color = Color.red;
                    break;
                }
                case ServerStatusType.Maintenance:
                {
                    serverNameText.color = Color.black;
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