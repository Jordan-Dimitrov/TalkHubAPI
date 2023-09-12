using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Interfaces.ForumInterfaces
{
    public interface IForumMessageRepository
    {
        bool AddForumMessage(ForumMessage message);
        bool RemoveForumMessage(ForumMessage message);
        bool UpdateForumMessage(ForumMessage message);
        bool ForumMessageExists(int id);
        bool Save();
        ForumMessage GetForumMessage(int id);
        bool ForumMessageExists(string name);
        ForumMessage GetForumMessageByName(string name);
        ICollection<ForumMessage> GetForumMessages();
        ICollection<ForumMessage> GetForumMessagesByForumThreadId(int forumThreadId);
        ICollection<ForumMessage> GetForumMessagesByUserId(int userId);
    }
}
