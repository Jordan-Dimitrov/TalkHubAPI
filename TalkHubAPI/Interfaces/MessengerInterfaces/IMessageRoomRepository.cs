using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IMessageRoomRepository
    {
        Task<bool> AddMessageRoomAsync(MessageRoom room);
        Task<bool> RemoveMessageRoomAsync(MessageRoom room);
        Task<bool> UpdateMessageRoomAsync(MessageRoom room);
        Task<bool> MessageRoomExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<MessageRoom?> GetMessageRoomAsync(int id);
        Task<bool> MessageRoomExistsAsync(string name);
        Task<MessageRoom?> GetMessageRoomByNameAsync(string name);
        Task<ICollection<MessageRoom>> GetMessageRoomsAsync();
        Task<ICollection<MessageRoom>> GetMessageRoomsForUserAsync(int userId);
    }
}
