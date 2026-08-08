// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler
{
    /// <summary>
    /// Celeritas 消息处理器的抽象基类。
    /// 实现 <see cref="ICeleritasHandler{T}"/> 接口的显式接口方法，
    /// 将 <c>object</c> 类型的 header 强转为强类型 <see cref="header"/> 后，
    /// 派发给子类实现的 <see cref="Handle(object, header, T)"/> 方法。
    /// </summary>
    /// <typeparam name="T">该处理器负责处理的 Protobuf 消息类型。</typeparam>
    public abstract class CeleritasHandlerBase<T> : ICeleritasHandler<T>
    {
        /// <inheritdoc/>
        void ICeleritasHandler<T>.Handle(object sender, object header, T message)
        {
            Handle(sender, (header)header, message);
        }

        /// <summary>
        /// 处理具体消息的抽象方法，由子类实现业务逻辑。
        /// </summary>
        /// <param name="sender">消息发送方。</param>
        /// <param name="header">消息公共头，包含路由等元信息。</param>
        /// <param name="message">具体的 Protobuf 消息体。</param>
        protected abstract void Handle(object sender, header header, T message);
    }
}
