// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     登录流程完成（数据加载完毕）响应处理器。
    /// </summary>
    public class LoginFinishResponseHandler : CeleritasHandlerBase<login_finish_response>
    {
        protected override void Handle(object sender, header header, login_finish_response message)
        {
            Log.Info("Login Finish.");

            GameEntry.ModuleComponent.LoginFinish();

            GameEntry.Event.Fire(this, LoginProgressEventArgs.Create(1f));
        }
    }
}