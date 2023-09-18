using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddForumThreadAsync(ForumThread thread)
        {
            _Context.Add(thread);
            return await SaveAsync();
        }

        public async Task<bool> ForumThreadExistsAsync(int id)
        {
            return await _Context.ForumThreads.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ForumThreadExistsAsync(string name)
        {
            return await _Context.ForumThreads.AnyAsync(x => x.ThreadName == name);
        }

        public async Task<ForumThread> GetForumThreadAsync(int id)
        {
            return await _Context.ForumThreads.FindAsync(id);
        }

        public async Task<ForumThread> GetForumThreadByNameAsync(string name)
        {
            return await _Context.ForumThreads.FirstOrDefaultAsync(x => x.ThreadName == name);
        }

        public async Task<ICollection<ForumThread>> GetForumThreadsAsync()
        {
            return await _Context.ForumThreads.ToListAsync();
        }

        public async Task<bool> RemoveForumThreadAsync(ForumThread thread)
        {
            _Context.Remove(thread);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateForumThreadAsync(ForumThread thread)
        {
            _Context.Update(thread);
            return await SaveAsync();
        }
    }
}
