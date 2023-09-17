using TalkHubAPI.Models.ForumModels;
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
        Task<MessageRoom> GetMessageRoomAsync(int id);
        Task<bool> MessageRoomExistsAsync(string name);
        Task<MessageRoom> GetMessageRoomByNameAsync(string name);
        Task<List<MessageRoom>> GetMessageRoomsAsync();
        Task<List<MessageRoom>> GetMessageRoomsForUserAsync(int userId);
    }
}
