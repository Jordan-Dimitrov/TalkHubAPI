using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Interfaces.MessengerInterfaces
{
    public interface IMessengerMessageRepository
    {
        bool AddMessengerMessage(MessengerMessage message);
        bool RemoveMessengerMessage(MessengerMessage message);
        bool UpdateMessengerMessage(MessengerMessage message);
        bool MessengerMessageExists(int id);
        bool Save();
        MessengerMessage GetMessengerMessage(int id);
        ICollection<MessengerMessage> GetMessengerMessages();
        ICollection<MessengerMessage> GetLastTenMessengerMessagesFromLastMessageId(int messageId, int roomId);
        ICollection<MessengerMessage> GetMessengerMessagesByRoomId(int roomId);
        ICollection<MessengerMessage> GetMessengerMessagesByUserId(int userId);
        MessengerMessage GetLastMessage();
    }
}
