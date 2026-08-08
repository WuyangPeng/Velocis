// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Mail;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    /// 邮件数据更新响应处理器。
    /// </summary>
    public class MailResponseHandler : CeleritasHandlerBase<mail_response>
    {
        private MailModule _mailModule;

        protected override void Handle(object sender, header header, mail_response message)
        {
            if (message == null || message.Mail.Count == 0)
            {
                Log.Info("MailResponseHandler: No mail data in response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            StoreMailData(message.Mail);
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
                Log.Warning("ModuleComponent is null in MailResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule != null)
            {
                return true;
            }

            Log.Warning("MailModule is null in MailResponseHandler.EnsureModule.");
            return false;

        }

        /// <summary>
        /// 批量存储邮件数据。
        /// </summary>
        /// <param name="mailList">邮件数据列表。</param>
        private void StoreMailData(RepeatedField<mail_data> mailList)
        {
            foreach (var mail in mailList)
            {
                try
                {
                    StoreMailData(mail);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to process mail data: {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// 存储单个邮件数据，并通知邮件模块。
        /// </summary>
        /// <param name="mail">单个邮件的详细数据结构。</param>
        private void StoreMailData(mail_data mail)
        {
            if (mail.MailId == 0)
            {
                Log.Warning("MailResponseHandler: ignoring mail data with no MailId: {0}", mail.ToString());
                return;
            }

            var mailData = new MailData(mail);
            _mailModule.UpdateMail(mailData);

            Log.Info("MailResponseHandler: Stored mail data for MailId={0}, Title={1}", mail.MailId, mail.Title);
        }
    }
}