// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Mail;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    /// 一键删除已读邮件的响应处理器。
    /// </summary>
    public class MailDeleteAllReadResponseHandler : CeleritasHandlerBase<mail_delete_all_read_response>
    {
        private MailModule _mailModule;

        protected override void Handle(object sender, header header, mail_delete_all_read_response message)
        {
            if (message == null || message.MailIds.Count == 0)
            {
                Log.Info("MailDeleteAllReadResponseHandler: No mail ids in delete all read response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            foreach (var mailId in message.MailIds)
            {
                _mailModule.DeleteItem(mailId);
            }

            Log.Info("MailDeleteAllReadResponseHandler: Deleted {0} read mails", message.MailIds.Count);
        }

        /// <summary>
        /// 确保并获取邮件模块实例。
        /// </summary>
        /// <returns>若邮件模块获取成功返回 true，否则返回 false。</returns>
        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailDeleteAllReadResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule != null)
            {
                return true;
            }

            Log.Warning("MailModule is null in MailDeleteAllReadResponseHandler.EnsureModule.");
            return false;
        }
    }
}