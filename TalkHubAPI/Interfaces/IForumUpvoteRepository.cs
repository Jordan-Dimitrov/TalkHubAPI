using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IForumUpvoteRepository
    {
        bool AddForumUpvote(ForumUpvote upvote);
        bool RemoveForumUpvote(ForumUpvote upvote);
        bool UpdateForumUpvote(ForumUpvote upvote);
        bool ForumUpvoteExists(int id);
        bool Save();
        ForumUpvote GetForumUpvote(int id);
        ICollection<ForumUpvote> GetForumUpvotes();
    }
}
