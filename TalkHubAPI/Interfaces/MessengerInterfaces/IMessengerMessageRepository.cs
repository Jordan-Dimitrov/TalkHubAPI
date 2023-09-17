using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IMessengerMessageRepository
    {
        Task<bool> AddMessengerMessageAsync(MessengerMessage message);
        Task<bool> RemoveMessengerMessageAsync(MessengerMessage message);
        Task<bool> UpdateMessengerMessageAsync(MessengerMessage message);
        Task<bool> MessengerMessageExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<MessengerMessage> GetMessengerMessageAsync(int id);
        Task<ICollection<MessengerMessage>> GetMessengerMessagesAsync();
        Task<ICollection<MessengerMessage>> GetLastTenMessengerMessagesFromLastMessageIdAsync(int messageId, int roomId);
        Task<ICollection<MessengerMessage>> GetMessengerMessagesByRoomIdAsync(int roomId);
        Task<ICollection<MessengerMessage>> GetMessengerMessagesByUserIdAsync(int userId);
        Task<MessengerMessage> GetLastMessageAsync();
    }
}
