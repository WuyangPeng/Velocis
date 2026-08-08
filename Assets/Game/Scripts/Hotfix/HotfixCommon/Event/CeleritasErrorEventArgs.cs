// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     Celeritas 网络请求错误事件参数。
    /// </summary>
    public class CeleritasErrorEventArgs : GameEventArgs
    {
        /// <summary>
        ///     事件编号。
        /// </summary>
        public static readonly int EventId = typeof(CeleritasErrorEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     获取 RPC 序列号。
        /// </summary>
        public int Rpc { get; private set; }

        /// <summary>
        ///     获取错误码。
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        ///     创建网络错误事件。
        /// </summary>
        /// <param name="rpc">RPC 序列号。</param>
        /// <param name="errorCode">错误码。</param>
        /// <returns>创建的事件参数。</returns>
        public static CeleritasErrorEventArgs Create(int rpc, int errorCode)
        {
            var eventArgs = new CeleritasErrorEventArgs
            {
                Rpc = rpc,
                ErrorCode = errorCode
            };
            return eventArgs;
        }

        /// <summary>
        ///     清理网络错误事件。
        /// </summary>
        public override void Clear()
        {
            Rpc = 0;
            ErrorCode = 0;
        }
    }
}
