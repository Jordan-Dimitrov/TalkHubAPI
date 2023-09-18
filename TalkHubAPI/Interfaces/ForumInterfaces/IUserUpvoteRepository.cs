using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Interfaces.ForumInterfaces
{
    public interface IUserUpvoteRepository
    {
        Task<bool> AddUserUpvoteAsync(UserUpvote upvote);
        Task<bool> RemoveUserUpvoteAsync(UserUpvote upvote);
        Task<bool> UpdateUserUpvoteAsync(UserUpvote upvote);
        Task<bool> UserUpvoteExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<bool> UserUpvoteExistsForMessageAndUserAsync(int messageId, int userId);
        Task<UserUpvote> GetUserUpvoteByMessageAndUserAsync(int messageId, int userId);
        Task<UserUpvote> GetUserUpvoteAsync(int id);
        Task<ICollection<UserUpvote>> GetUserUpvotesAsync();
    }
}
