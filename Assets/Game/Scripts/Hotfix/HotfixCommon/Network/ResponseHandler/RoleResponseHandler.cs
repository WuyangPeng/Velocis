// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Role;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    /// 角色数据响应处理器。
    /// </summary>
    public class RoleResponseHandler : CeleritasHandlerBase<role_response>
    {
        protected override void Handle(object sender, header header, role_response message)
        {
            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();

            roleModule.SetRole(message);

            GameEntry.Event.Fire(this, ChangeNameEventArgs.Create());
        }
    }
}