using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IMessageRoomRepository
    {
        bool AddMessageRoom(MessageRoom room);
        bool RemoveMessageRoom(MessageRoom room);
        bool UpdateMessageRoom(MessageRoom room);
        bool MessageRoomExists(int id);
        bool Save();
        MessageRoom GetMessageRoom(int id);
        bool MessageRoomExists(string name);
        MessageRoom GetMessageRoomByName(string name);
        ICollection<MessageRoom> GetMessageRooms();
        ICollection<MessageRoom> GetMessageRoomsForUser(int userId);
    }
}
