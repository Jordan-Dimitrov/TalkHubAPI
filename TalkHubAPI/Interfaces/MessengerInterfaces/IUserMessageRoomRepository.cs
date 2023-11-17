using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IUserMessageRoomRepository
    {
        Task<bool> AddUserMessageRoomAsync(UserMessageRoom userMessageRoom);
        Task<bool> RemoveUserMessageRoomAsync(UserMessageRoom userMessageRoom);
        Task<bool> UpdateUserMessageRoomAsync(UserMessageRoom userMessageRoom);
        Task<bool> UserMessageRoomExistsAsync(int id);
        Task<bool> UserMessageRoomExistsForRoomAndUserAsync(int roomId, int userId);
        Task<UserMessageRoom?> GetUserMessageRoomAsync(int id);
        Task<UserMessageRoom?> GetUserMessageRoomForRoomAndUserAsync(int roomId, int userId);
        Task<ICollection<UserMessageRoom>> GetUserMessageRoomsAsync();
        Task<ICollection<UserMessageRoom>> GetUserMessageRoomsAsyncForRoom(int roomId);
        Task<bool> RemoveUserMessageRoomForRoomId(int roomId);
        Task<bool> SaveAsync();

    }
}
