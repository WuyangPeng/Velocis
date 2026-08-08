// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Debug;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Game.Scripts.Main.Runtime.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     调试信息响应处理器。
    /// </summary>
    public class DebugInfoResponseHandler : CeleritasHandlerBase<debug_info_response>
    {
        protected override void Handle(object sender, header header, debug_info_response message)
        {
            var debugModule = GameEntry.ModuleComponent.GetModule<DebugModule>();
            debugModule.IsDebug = message.OpenDebug;

            GameEntry.Event.Fire(this, DebugInfoEventArgs.Create());
        }
    }
}