using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Repository.ForumRepositories
{
    public class ForumThreadRepository : IForumThreadRepository
    {
        private readonly TalkHubContext _Context;

        public ForumThreadRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public bool AddForumThread(ForumThread thread)
        {
            _Context.Add(thread);
            return Save();
        }

        public bool ForumThreadExists(int id)
        {
            return _Context.ForumThreads.Any(x => x.Id == id);
        }

        public bool ForumThreadExists(string name)
        {
            return _Context.ForumThreads.Any(x => x.ThreadName == name);
        }

        public ForumThread GetForumThread(int id)
        {
            return _Context.ForumThreads.Find(id);
        }

        public ForumThread GetForumThreadByName(string name)
        {
            return _Context.ForumThreads.FirstOrDefault(x => x.ThreadName == name);
        }

        public ICollection<ForumThread> GetForumThreads()
        {
            return _Context.ForumThreads.ToList();
        }

        public bool RemoveForumThread(ForumThread thread)
        {
            _Context.Remove(thread);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateForumThread(ForumThread thread)
        {
            _Context.Update(thread);
            return Save();
        }
    }
}
