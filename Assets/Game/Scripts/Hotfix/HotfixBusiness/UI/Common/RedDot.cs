// 创建时间：2026-07-23
// 修改时间：2026-08-01
// 审核时间：

using Celeritas.Config;
using Celeritas.Config.game;
using Game.Scripts.Hotfix.HotfixCommon.Config;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot;
using Game.Scripts.Main.Runtime.Base;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class RedDot : MonoBehaviour
    {
        [SerializeField] private red_dot_type redDotType;
        [SerializeField] private Image redDotImage;
        [SerializeField] private TMP_Text countText;

        private int _value;

        public red_dot_type RedDotType => redDotType;
        public Image RedDotImage => redDotImage;
        public TMP_Text CountText => countText;

        private void Awake()
        {
            if (GameEntry.Event != null)
            {
                GameEntry.Event.Subscribe(ChangeRedDotEventArgs.EventId, OnChangeRedDotSuccess);
            }
        }

        private void OnEnable()
        {
            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            if (redDotModule != null)
            {
                _value = redDotModule.GetRedDotNodeValue(redDotType);
            }

            Refresh();
        }

        public void SetCount(int count)
        {
            _value = count;
            Refresh();
        }

        public void SetVisible(bool visible)
        {
            _value = visible ? 1 : 0;
            Refresh();
        }

        private void OnClose()
        {
            if (GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(ChangeRedDotEventArgs.EventId, OnChangeRedDotSuccess);
            }
        }

        private void OnChangeRedDotSuccess(object sender, GameEventArgs e)
        {
            var changeRedDotEventArgs = (ChangeRedDotEventArgs)e;
            if (changeRedDotEventArgs.RedDot.TryGetValue(redDotType, out _value))
            {
                Refresh();
            }
        }

        private red_dot_config GetConfig()
        {
            if (GameEntry.GameConfig != null)
            {
                var tables = GameEntry.GameConfig.GetTables();
                if (tables != null && tables.RedDotConfigContainer != null)
                {
                    return tables.RedDotConfigContainer.GetOrDefault(redDotType);
                }
            }

            return null;
        }

        private void Refresh()
        {
            var isShow = _value > 0;

            if (gameObject.activeSelf != isShow)
            {
                gameObject.SetActive(isShow);
            }

            if (redDotImage != null)
            {
                redDotImage.gameObject.SetActive(isShow);
            }

            if (countText == null)
            {
                return;
            }

            var config = GetConfig();
            if (isShow && config != null && config.RedDotStatusType == red_dot_status_type.sum)
            {
                countText.gameObject.SetActive(true);
                countText.text = _value > 99 ? "99+" : _value.ToString();
            }
            else
            {
                countText.gameObject.SetActive(false);
            }
        }
    }
}