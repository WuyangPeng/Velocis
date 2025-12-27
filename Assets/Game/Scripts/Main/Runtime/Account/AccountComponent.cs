using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Account
{
    public class AccountComponent : GameFrameworkComponent
    {
        [Header("ServerType")] [SerializeField]
        public ServerType serverType;

        [Header("Intranet")] [SerializeField] private string intranetGuestLoginUrl;

        [SerializeField] private string intranetServerListUrl;

        [Header("External")] [SerializeField] private string externalGuestLoginUrl;

        [SerializeField] private string externalServerListUrl;

        [Header("App")] [SerializeField] public string appId;

        [SerializeField] public string secret;

        [SerializeField] public string appVersion;

        public string GuestLoginUrl => serverType == ServerType.Intranet ? intranetGuestLoginUrl : externalGuestLoginUrl;
        public string ServerListUrl => serverType == ServerType.Intranet ? intranetServerListUrl : externalServerListUrl;
    }
}