using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class UserSubscribtionRepository : IUserSubscribtionRepository
    {
        private readonly TalkHubContext _Context;
        public UserSubscribtionRepository(TalkHubContext context) 
        {
            _Context = context;
        }
        public async Task<bool> AddUserSubscribtionAsync(UserSubscribtion userSubscribtion)
        {
            await _Context.AddAsync(userSubscribtion);
            return await SaveAsync();
        }

        public async Task<UserSubscribtion?> GetUserSubscribtionAsync(int id)
        {
            return await _Context.UserSubscribtions.FindAsync(id);
        }

        public async Task<UserSubscribtion?> GetUserSubscribtionByChannelAndSubscriberAsync(int channelId, int subscriberId)
        {
            return await _Context.UserSubscribtions
            .Where(x => x.UserChannelId == channelId && x.UserSubscriberId == subscriberId)
            .FirstOrDefaultAsync();
        }

        public async Task<ICollection<UserSubscribtion>> GetUserSubscribtionsAsync()
        {
            return await _Context.UserSubscribtions.ToListAsync();
        }

        public async Task<bool> RemoveUserSubscribtionAsync(UserSubscribtion userSubscribtion)
        {
            _Context.Remove(userSubscribtion);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUserSubscribtionAsync(UserSubscribtion userSubscribtion)
        {
            _Context.Update(userSubscribtion);
            return await SaveAsync();
        }

        public async Task<bool> UserSubscribtionExistsAsync(int id)
        {
            return await _Context.UserSubscribtions.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> UserSubscribtionExistsForChannelAndSubscriberAsync(int channelId, int subscriberId)
        {
            return await _Context.UserSubscribtions
                .AnyAsync(x => x.UserChannelId == channelId && x.UserSubscriberId == subscriberId);
        }
    }
}
