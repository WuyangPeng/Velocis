// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System.Collections.Generic;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Game.Scripts.Main.Runtime.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    /// 红点数据状态更新响应处理器。
    /// </summary>
    public class RedDotResponseHandler : CeleritasHandlerBase<red_dot_response>
    {
        protected override void Handle(object sender, header header, red_dot_response message)
        {
            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            if (message.IsLogin)
            {
                redDotModule.ClearRedDotNode();
            }

            Dictionary<red_dot_type, int> redDot = new();
            foreach (var element in message.Node)
            {
                redDotModule.AddRedDotNode(new RedDotNode((red_dot_type)element.RedDotType, element.Value));
                if (!message.IsLogin)
                {
                    redDot[(red_dot_type)element.RedDotType] = element.Value;
                }
            }

            if (!message.IsLogin)
            {
                GameEntry.Event.Fire(this, ChangeRedDotEventArgs.Create(redDot));
            }
        }
    }
}