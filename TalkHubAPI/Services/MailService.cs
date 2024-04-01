using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ConfigurationModels;

namespace TalkHubAPI.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _MailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _MailSettings = mailSettingsOptions.Value;
        }
        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_MailSettings.SenderName, _MailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_MailSettings.Server, _MailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_MailSettings.UserName, _MailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
