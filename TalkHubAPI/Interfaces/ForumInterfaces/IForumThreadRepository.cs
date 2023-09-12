using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Interfaces.ForumInterfaces
{
    public interface IForumThreadRepository
    {
        bool AddForumThread(ForumThread thread);
        bool RemoveForumThread(ForumThread thread);
        bool UpdateForumThread(ForumThread thread);
        bool ForumThreadExists(int id);
        bool Save();
        ForumThread GetForumThread(int id);
        ICollection<ForumThread> GetForumThreads();
        bool ForumThreadExists(string name);
        ForumThread GetForumThreadByName(string name);
    }
}
