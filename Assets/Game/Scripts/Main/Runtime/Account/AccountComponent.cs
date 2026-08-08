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

        [SerializeField] private string intranetAnnouncementUrl;

        [SerializeField] private string intranetFeedbackUrl;

        [Header("External")] [SerializeField] private string externalGuestLoginUrl;

        [SerializeField] private string externalServerListUrl;

        [SerializeField] private string externalAnnouncementUrl;

        [SerializeField] private string externalFeedbackUrl;

        [Header("App")] [SerializeField] public string appId;

        [SerializeField] public string secret;

        [SerializeField] public string appVersion;

        public string GuestLoginUrl => serverType == ServerType.Intranet ? intranetGuestLoginUrl : externalGuestLoginUrl;
        public string ServerListUrl => serverType == ServerType.Intranet ? intranetServerListUrl : externalServerListUrl;
        public string AnnouncementUrl => serverType == ServerType.Intranet ? intranetAnnouncementUrl : externalAnnouncementUrl;
        public string FeedbackUrl => serverType == ServerType.Intranet ? intranetFeedbackUrl : externalFeedbackUrl;
    }
}