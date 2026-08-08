// 创建时间：2026-07-27
// 修改时间：2026-07-27

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Scripts.Hotfix.HotfixCommon.Login;
using Game.Scripts.Main.Runtime.UI.UICommon;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    /// <summary>
    /// 服务器列表单个卡片组件。由 ServerListItemCreator 反射绑定序列化字段。
    /// </summary>
    public class ServerListItem : MonoBehaviour
    {
        [SerializeField] private Image imageBackground;
        [SerializeField] private TMP_Text serverNameText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Image statusIndicator;
        [SerializeField] private TMP_Text pingText;
        [SerializeField] private GameObject characterMark;
        [SerializeField] private BaseButton button;
        [SerializeField] private GameObject selectFrame;

        public Image ImageBackground => imageBackground;
        public TMP_Text ServerNameText => serverNameText;
        public TMP_Text PlayerNameText => playerNameText;
        public Image StatusIndicator => statusIndicator;
        public TMP_Text PingText => pingText;
        public GameObject CharacterMark => characterMark;
        public BaseButton Button => button;
        public GameObject SelectFrame => selectFrame;

        public void SetSelected(bool selected)
        {
            if (selectFrame != null)
            {
                selectFrame.SetActive(selected);
            }
        }

        public void SetData(LoginServerInfo loginServerInfo)
        {
            if (serverNameText != null)
            {
                serverNameText.enableWordWrapping = false;
                serverNameText.overflowMode = TextOverflowModes.Ellipsis;
                serverNameText.text = loginServerInfo.GetDisplayServerName();
            }

            if (playerNameText != null)
            {
                playerNameText.enableWordWrapping = false;
                playerNameText.overflowMode = TextOverflowModes.Ellipsis;
                playerNameText.text = loginServerInfo.GetPlayerName();
            }
        }
    }
}
