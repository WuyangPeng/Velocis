// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    /// 登录服务器响应处理器，用以同步服务器当前时间及用户 ID。
    /// </summary>
    public class LoginResponseHandler : CeleritasHandlerBase<login_response>
    {
        protected override void Handle(object sender, header header, login_response message)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetServerTime(message.CurrentTime);
            userModule.SetUserId(header.ToGateway.UserId);

            Log.Info("login_.CurrentTime ='{0}'.", message.CurrentTime);

            GameEntry.Event.Fire(this, LoginProgressEventArgs.Create(0.5f));
        }
    }
}