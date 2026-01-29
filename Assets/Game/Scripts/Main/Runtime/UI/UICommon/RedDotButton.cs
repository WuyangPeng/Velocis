using Celeritas.Config;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.RedDot;
using GameFramework.Event;
using UnityEngine;
using static Game.Scripts.Main.Runtime.Event.ChangeRedDotEventArgs;

namespace Game.Scripts.Main.Runtime.UI.UICommon
{
    public class RedDotButton : MonoBehaviour
    {
        [SerializeField] private red_dot_type redDotType;

        private int value;

        private void Awake()
        {
            GameEntry.Event.Subscribe(EventId, OnChangeRedDotSuccess);

            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            value = redDotModule.GetRedDotNodeValue(redDotType);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(EventId, OnChangeRedDotSuccess);
        }

        private void OnChangeRedDotSuccess(object sender, GameEventArgs e)
        {
            var changeRedDotEventArgs = (ChangeRedDotEventArgs)e;
            if (changeRedDotEventArgs.RedDot.TryGetValue(redDotType, out value))
            {
            }
        }
    }
}