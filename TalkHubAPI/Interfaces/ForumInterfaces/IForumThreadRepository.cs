using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Interfaces.ForumInterfaces
{
    public interface IForumThreadRepository
    {
        Task<bool> AddForumThreadAsync(ForumThread thread);
        Task<bool> RemoveForumThreadAsync(ForumThread thread);
        Task<bool> UpdateForumThreadAsync(ForumThread thread);
        Task<bool> ForumThreadExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<ForumThread?> GetForumThreadAsync(int id);
        Task<ICollection<ForumThread>> GetForumThreadsAsync();
        Task<bool> ForumThreadExistsAsync(string name);
        Task<ForumThread?> GetForumThreadByNameAsync(string name);
    }
}
