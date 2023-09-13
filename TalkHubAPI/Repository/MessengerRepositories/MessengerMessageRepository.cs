using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Repository.MessengerRepositories
{
    public class MessengerMessageRepository : IMessengerMessageRepository
    {
        private readonly TalkHubContext _Context;
        private readonly int _MessagesToRetrieve;

        public MessengerMessageRepository(TalkHubContext context)
        {
            _Context = context;
            _MessagesToRetrieve = 10;
        }
        public bool AddMessengerMessage(MessengerMessage message)
        {
            _Context.Add(message);
            return Save();
        }

        public MessengerMessage GetLastMessage()
        {
            return _Context.MessengerMessages.OrderBy(x => x.Id).FirstOrDefault();
        }

        public ICollection<MessengerMessage> GetLastTenMessengerMessagesFromLastMessageId(int messageId, int roomId)
        {
            return _Context.MessengerMessages
                .Where(x => x.RoomId == roomId && x.Id < messageId)
                .OrderByDescending(x => x.Id)
                .Take(_MessagesToRetrieve)
                .OrderBy(x => x.Id)
                .ToList();
        }

        public MessengerMessage GetMessengerMessage(int id)
        {
            return _Context.MessengerMessages.Find(id);
        }

        public ICollection<MessengerMessage> GetMessengerMessages()
        {
            return _Context.MessengerMessages.ToList();
        }

        public ICollection<MessengerMessage> GetMessengerMessagesByRoomId(int roomId)
        {
            return _Context.MessengerMessages.Where(x => x.RoomId == roomId).ToList();
        }

        public ICollection<MessengerMessage> GetMessengerMessagesByUserId(int userId)
        {
            return _Context.MessengerMessages.Where(x => x.UserId == userId).ToList();
        }

        public bool MessengerMessageExists(int id)
        {
            return _Context.MessengerMessages.Any(x => x.Id == id);
        }

        public bool RemoveMessengerMessage(MessengerMessage message)
        {
            _Context.Remove(message);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMessengerMessage(MessengerMessage message)
        {
            _Context.Update(message);
            return Save();
        }
    }
}
