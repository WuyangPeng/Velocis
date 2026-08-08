using System.Reflection;
using Game.Scripts.Main.Runtime.Account;
using NUnit.Framework;
using UnityEngine;

namespace Game.Scripts.Tests.Edit.Account
{
    /// <summary>
    ///     针对 AccountComponent 的单元测试类。
    /// </summary>
    public class AccountComponentTest
    {
        /// <summary>
        ///     测试 AccountComponent 是否能根据 serverType 正确切换登录和服务器列表的 URL。
        /// </summary>
        [Test]
        public void TestUrlSwitchingBasedOnServerType()
        {
            // 创建一个临时的游戏对象，并挂载 AccountComponent 以便进行测试。
            var gameObject = new GameObject("AccountComponentTestObject");
            var accountComponent = gameObject.AddComponent<AccountComponent>();

            // 定义用于测试的内网和外网 URL。
            // 在真实场景中，这些值会由 Unity 编辑器的 Inspector 面板设置。
            const string intranetGuestLoginUrl = "http://intranet.test/guest_login";
            const string intranetServerListUrl = "http://intranet.test/server_list";
            const string intranetAnnouncementUrl = "http://intranet.test/announcements";
            const string externalGuestLoginUrl = "http://external.test/guest_login";
            const string externalServerListUrl = "http://external.test/server_list";
            const string externalAnnouncementUrl = "http://external.test/announcements";

            // 由于这些 URL 字段是私有的 (private)，我们需要使用 C# 的反射 (Reflection) 机制来在测试中给它们赋值。
            var componentType = typeof(AccountComponent);
            componentType.GetField("intranetGuestLoginUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, intranetGuestLoginUrl);
            componentType.GetField("intranetServerListUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, intranetServerListUrl);
            componentType.GetField("intranetAnnouncementUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, intranetAnnouncementUrl);
            componentType.GetField("externalGuestLoginUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, externalGuestLoginUrl);
            componentType.GetField("externalServerListUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, externalServerListUrl);
            componentType.GetField("externalAnnouncementUrl", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(accountComponent, externalAnnouncementUrl);

            accountComponent.serverType = ServerType.Intranet;

            Assert.AreEqual(intranetGuestLoginUrl, accountComponent.GuestLoginUrl, "当 ServerType 为 Intranet 时，GuestLoginUrl 应该返回内网地址。");
            Assert.AreEqual(intranetServerListUrl, accountComponent.ServerListUrl, "当 ServerType 为 Intranet 时，ServerListUrl 应该返回内网地址。");
            Assert.AreEqual(intranetAnnouncementUrl, accountComponent.AnnouncementUrl, "当 ServerType 为 Intranet 时，AnnouncementUrl 应该返回内网地址。");

            accountComponent.serverType = ServerType.External;

            Assert.AreEqual(externalGuestLoginUrl, accountComponent.GuestLoginUrl, "当 ServerType 为 External 时，GuestLoginUrl 应该返回外网地址。");
            Assert.AreEqual(externalServerListUrl, accountComponent.ServerListUrl, "当 ServerType 为 External 时，ServerListUrl 应该返回外网地址。");
            Assert.AreEqual(externalAnnouncementUrl, accountComponent.AnnouncementUrl, "当 ServerType 为 External 时，AnnouncementUrl 应该返回外网地址。");

            Object.DestroyImmediate(gameObject);
        }
    }
}
