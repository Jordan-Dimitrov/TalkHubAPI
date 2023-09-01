using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IUserUpvoteRepository
    {
        bool AddUserUpvote(UserUpvote upvote);
        bool RemoveUserUpvote(UserUpvote upvote);
        bool UpdateUserUpvote(UserUpvote upvote);
        bool UserUpvoteExists(int id);
        bool Save();
        bool UserUpvoteExistsForMessageAndUser(int messageId, int userId);
        UserUpvote GetUserUpvoteByMessageAndUser(int messageId, int userId);
        UserUpvote GetUserUpvote(int id);
        ICollection<UserUpvote> GetUserUpvotes();
    }
}
