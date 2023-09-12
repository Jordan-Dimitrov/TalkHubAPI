using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Repository.MessengerRepositories
{
    public class MessageRoomRepository : IMessageRoomRepository
    {
        public bool AddMessageRoom(MessageRoom room)
        {
            throw new NotImplementedException();
        }

        public MessageRoom GetMessageRoom(int id)
        {
            throw new NotImplementedException();
        }

        public MessageRoom GetMessageRoomByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<MessageRoom> GetMessageRooms()
        {
            throw new NotImplementedException();
        }

        public bool MessageRoomExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool MessageRoomExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMessageRoom(MessageRoom room)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateMessageRoom(MessageRoom room)
        {
            throw new NotImplementedException();
        }
    }
}
