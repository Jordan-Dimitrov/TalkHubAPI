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
        UserUpvote GetUserUpvote(int id);
        ICollection<UserUpvote> GetUserUpvotes();
    }
}
