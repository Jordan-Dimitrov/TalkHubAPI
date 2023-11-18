using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces.ServiceInterfaces
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
