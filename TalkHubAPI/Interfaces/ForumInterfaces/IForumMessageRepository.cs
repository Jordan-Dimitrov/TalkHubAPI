using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Interfaces.ForumInterfaces
{
    public interface IForumMessageRepository
    {
        Task<bool> AddForumMessageAsync(ForumMessage message);
        Task<bool> RemoveForumMessageAsync(ForumMessage message);
        Task<bool> UpdateForumMessageAsync(ForumMessage message);
        Task<bool> ForumMessageExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<ForumMessage?> GetForumMessageAsync(int id);
        Task<bool> ForumMessageExistsAsync(string name);
        Task<ForumMessage?> GetForumMessageByNameAsync(string name);
        Task<ICollection<ForumMessage>> GetForumMessagesAsync();
        Task<ICollection<ForumMessage>> GetForumMessagesByForumThreadIdAsync(int forumThreadId);
        Task<ICollection<ForumMessage>> GetForumMessagesByUserIdAsync(int userId);
    }
}
