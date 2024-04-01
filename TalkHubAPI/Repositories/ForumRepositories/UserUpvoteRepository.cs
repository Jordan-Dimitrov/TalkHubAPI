using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Repositories.ForumRepositories
{
    public class UserUpvoteRepository : IUserUpvoteRepository
    {
        private readonly TalkHubContext _Context;

        public UserUpvoteRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddUserUpvoteAsync(UserUpvote upvote)
        {
            await _Context.AddAsync(upvote);
            return await SaveAsync();
        }

        public async Task<UserUpvote?> GetUserUpvoteAsync(int id)
        {
            return await _Context.UserUpvotes.Include(x => x.User)
                .Include(x => x.Message).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserUpvote?> GetUserUpvoteByMessageAndUserAsync(int messageId, int userId)
        {
            return await _Context.UserUpvotes.Include(x => x.User).Include(x => x.Message)
                .FirstOrDefaultAsync(x => x.MessageId == messageId && x.UserId == userId);
        }

        public async Task<ICollection<UserUpvote>> GetUserUpvotesAsync()
        {
            return await _Context.UserUpvotes.ToListAsync();
        }

        public async Task<bool> RemoveUserUpvoteAsync(UserUpvote upvote)
        {
            _Context.Remove(upvote);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUserUpvoteAsync(UserUpvote upvote)
        {
            _Context.Update(upvote);
            return await SaveAsync();
        }

        public async Task<bool> UserUpvoteExistsAsync(int id)
        {
            return await _Context.UserUpvotes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> UserUpvoteExistsForMessageAndUserAsync(int messageId, int userId)
        {
            return await _Context.UserUpvotes.AnyAsync(x => x.MessageId == messageId && x.UserId == userId);
        }
    }
}
