using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Account
{
    public class AccountComponent : GameFrameworkComponent
    {
        [SerializeField] public string guestLoginUrl;

        [SerializeField] public string serverListUrl;

        [SerializeField] public string appId;

        [SerializeField] public string secret;

        public Token Token { get; private set; }
    }
}