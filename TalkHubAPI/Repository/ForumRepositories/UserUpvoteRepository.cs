using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Repository.ForumRepositories
{
    public class UserUpvoteRepository : IUserUpvoteRepository
    {
        private readonly TalkHubContext _Context;

        public UserUpvoteRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddUserUpvote(UserUpvote upvote)
        {
            _Context.Add(upvote);
            return Save();
        }

        public UserUpvote GetUserUpvote(int id)
        {
            return _Context.UserUpvotes.Find(id);
        }

        public UserUpvote GetUserUpvoteByMessageAndUser(int messageId, int userId)
        {
            return _Context.UserUpvotes.Where(x => x.MessageId == messageId && x.UserId == userId).FirstOrDefault();
        }

        public ICollection<UserUpvote> GetUserUpvotes()
        {
            return _Context.UserUpvotes.ToList();
        }

        public bool RemoveUserUpvote(UserUpvote upvote)
        {
            _Context.Remove(upvote);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserUpvote(UserUpvote upvote)
        {
            _Context.Update(upvote);
            return Save();
        }

        public bool UserUpvoteExists(int id)
        {
            return _Context.UserUpvotes.Any(x => x.Id == id);
        }

        public bool UserUpvoteExistsForMessageAndUser(int messageId, int userId)
        {
            return _Context.UserUpvotes.Any(x => x.MessageId == messageId && x.UserId == userId);
        }
    }
}
