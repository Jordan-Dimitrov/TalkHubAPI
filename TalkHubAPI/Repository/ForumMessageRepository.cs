using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Repository
{
    public class ForumMessageRepository : IForumMessageRepository
    {
        private readonly TalkHubContext _Context;

        public ForumMessageRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public bool AddForumMessage(ForumMessage message)
        {
            _Context.Add(message);
            return Save();
        }

        public bool ForumMessageExists(int id)
        {
            return _Context.ForumMessages.Any(x => x.Id == id);
        }

        public bool ForumMessageExists(string name)
        {
            return _Context.ForumMessages.Any(x => x.MessageContent == name);
        }

        public ForumMessage GetForumMessage(int id)
        {
            return _Context.ForumMessages.Find(id);
        }

        public ForumMessage GetForumMessageByName(string name)
        {
            return _Context.ForumMessages.FirstOrDefault(x => x.MessageContent == name);
        }

        public ICollection<ForumMessage> GetForumMessages()
        {
            return _Context.ForumMessages.ToList();
        }

        public ICollection<ForumMessage> GetForumMessagesByForumThreadId(int forumThreadId)
        {
            return _Context.ForumMessages.Where(x => x.ForumThreadId == forumThreadId).ToList();
        }

        public ICollection<ForumMessage> GetForumMessagesByUserId(int userId)
        {
            return _Context.ForumMessages.Where(x => x.UserId == userId).ToList();
        }

        public bool RemoveForumMessage(ForumMessage message)
        {
            _Context.Remove(message);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateForumMessage(ForumMessage message)
        {
            _Context.Update(message);
            return Save();
        }
    }
}
