// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.Game;
using Game.Scripts.Hotfix.HotfixCommon.Network.Packet;
using Game.Scripts.Main.Runtime.Network;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler
{
    /// <summary>
    ///     服务端 → 客户端数据包 <see cref="SCCeleritas" /> 的处理器。
    ///     根据 <c>ToGateway.Code</c> 判断是正常消息还是错误响应：
    ///     <list type="bullet">
    ///         <item>
    ///             <description><see cref="GameErrorType.Success" />：转发给对应的 <see cref="celeritas" /> 处理器处理。</description>
    ///         </item>
    ///         <item>
    ///             <description>其他：弹出服务器错误提示弹窗。</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class SCCeleritasHandler : PacketHandlerBase
    {
        /// <summary>对应 <see cref="SCCeleritas.Id" />，用于注册 to 网络层路由。</summary>
        public override int Id => 101;

        /// <summary>
        ///     处理收到的 <see cref="SCCeleritas" /> 数据包。
        /// </summary>
        /// <param name="sender">网络发送方。</param>
        /// <param name="packet">原始数据包，将被强转为 <see cref="SCCeleritas" />。</param>
        public override void Handle(object sender, GameFramework.Network.Packet packet)
        {
            var packetImpl = (SCCeleritas)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());

            if ((GameErrorType)packetImpl.Common.ToGateway.Code == GameErrorType.Success)
            {
                HandleCeleritas(sender, packetImpl);
            }
            else
            {
                HandleError(sender, packetImpl);
            }
        }

        /// <summary>
        ///     Code 为 <see cref="GameErrorType.Success" /> 时，将消息转发给注册的 <see cref="celeritas" /> 处理器。
        /// </summary>
        /// <param name="sender">网络发送方。</param>
        /// <param name="packetImpl">已解析的 <see cref="SCCeleritas" /> 数据包。</param>
        private static void HandleCeleritas(object sender, SCCeleritas packetImpl)
        {
            var handler = GameEntry.CeleritasHandler.GetCeleritasHandler<celeritas>();
            if (handler != null)
            {
                Log.Info("Receive packet rpc ='{0}',message ={1}.", packetImpl.Common.ToGateway.Rpc, packetImpl.Celeritas.ToString());
                handler.Handle(sender, packetImpl.Common, packetImpl.Celeritas);
            }
            else
            {
                Log.Error("Can not find handler for 'celeritas'.");
            }
        }

        /// <summary>
        ///     Code 不为 <see cref="GameErrorType.Success" /> 时，弹出服务器错误提示弹窗并派发错误事件。
        /// </summary>
        /// <param name="sender">网络发送方。</param>
        /// <param name="packetImpl">已解析的 <see cref="SCCeleritas" /> 数据包。</param>
        private static void HandleError(object sender, SCCeleritas packetImpl)
        {
            Log.Info("Receive packet Code ='{0}'.", packetImpl.Common.ToGateway.Code);

            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Server.Error"),
                Message = GameEntry.Localization.GetString("Server.ErrorCode" +
                                                           packetImpl.Common.ToGateway.Code),
                OnClickConfirm = _ =>
                {
                    GameEntry.Event.Fire(sender, CeleritasErrorEventArgs.Create(packetImpl.Common.ToGateway.Rpc, packetImpl.Common.ToGateway.Code));
                }
            });
        }
    }
}