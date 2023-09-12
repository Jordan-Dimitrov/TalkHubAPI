using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Repository.MessengerRepositories
{
    public class MessengerMessageRepository : IMessengerMessageRepository
    {
        public bool AddMessengerMessage(MessengerMessage message)
        {
            throw new NotImplementedException();
        }

        public MessengerMessage GetMessengerMessage(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<MessengerMessage> GetMessengerMessages()
        {
            throw new NotImplementedException();
        }

        public ICollection<MessengerMessage> GetMessengerMessagesByRoomId(int roomId)
        {
            throw new NotImplementedException();
        }

        public ICollection<MessengerMessage> GetMessengerMessagesByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public bool MessengerMessageExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMessengerMessage(MessengerMessage message)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateMessengerMessage(MessengerMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
