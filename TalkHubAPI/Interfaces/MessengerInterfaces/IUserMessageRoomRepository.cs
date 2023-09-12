using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IUserMessageRoomRepository
    {
        bool AddUserMessageRoom(UserMessageRoom userMessageRoom);
        bool RemoveUserMessageRoom(UserMessageRoom userMessageRoom);
        bool UpdateUserMessageRoom(UserMessageRoom userMessageRoom);
        bool UserMessageRoomExists(int id);
        bool Save();
        UserMessageRoom GetUserMessageRoom(int id);
        ICollection<UserMessageRoom> GetUserMessageRooms();
    }
}
