using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IUserSubscribtionRepository
    {
        Task<bool> AddUserSubscribtionAsync(UserSubscribtion userSubscribtion);
        Task<bool> RemoveUserSubscribtionAsync(UserSubscribtion userSubscribtion);
        Task<bool> UpdateUserSubscribtionAsync(UserSubscribtion userSubscribtion);
        Task<bool> UserSubscribtionExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<bool> UserSubscribtionExistsForChannelAndSubscriberAsync(int channelId, int subscriberId);
        Task<UserSubscribtion?> GetUserSubscribtionByChannelAndSubscriberAsync(int channelId, int subscriberId);
        Task<UserSubscribtion?> GetUserSubscribtionAsync(int id);
        Task<ICollection<UserSubscribtion>> GetUserSubscribtionsAsync();
    }
}
