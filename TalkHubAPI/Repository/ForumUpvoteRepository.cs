using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Repository
{
    public class ForumUpvoteRepository : IForumUpvoteRepository
    {
        private readonly TalkHubContext _Context;

        public ForumUpvoteRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddForumUpvote(ForumUpvote upvote)
        {
            _Context.Add(upvote);
            return Save();
        }

        public bool ForumUpvoteExists(int id)
        {
            return _Context.ForumUpvotes.Any(x => x.Id == id);
        }

        public ForumUpvote GetForumUpvote(int id)
        {
            return _Context.ForumUpvotes.Find(id);
        }

        public ICollection<ForumUpvote> GetForumUpvotes()
        {
            return _Context.ForumUpvotes.ToList();
        }

        public bool RemoveForumUpvote(ForumUpvote upvote)
        {
            _Context.Remove(upvote);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateForumUpvote(ForumUpvote upvote)
        {
            _Context.Update(upvote);
            return Save();
        }
    }
}
