// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     通用调试响应处理器。
    /// </summary>
    public class DebugResponseHandler : CeleritasHandlerBase<debug_response>
    {
        protected override void Handle(object sender, header header, debug_response message)
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Button.Debug"),
                Message = GameEntry.Localization.GetString("Home.OperationSuccessful")
            });
        }
    }
}