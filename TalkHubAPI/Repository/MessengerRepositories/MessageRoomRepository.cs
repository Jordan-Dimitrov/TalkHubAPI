using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Repository.MessengerRepositories
{
    public class MessageRoomRepository : IMessageRoomRepository
    {
        private readonly TalkHubContext _Context;

        public MessageRoomRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddMessageRoom(MessageRoom room)
        {
            _Context.Add(room);
            return Save();
        }

        public MessageRoom GetMessageRoom(int id)
        {
            return _Context.MessageRooms.Find(id);
        }

        public MessageRoom GetMessageRoomByName(string name)
        {
            return _Context.MessageRooms.FirstOrDefault(x => x.RoomName == name);
        }

        public ICollection<MessageRoom> GetMessageRooms()
        {
            return _Context.MessageRooms.ToList();
        }

        public ICollection<MessageRoom> GetMessageRoomsForUser(int userId)
        {
            return _Context.MessageRooms
                .Where(x => x.UserMessageRooms.Any(a => a.UserId == userId))
                .Include(x => x.MessengerMessages)
                .ToList();
        }

        public bool MessageRoomExists(int id)
        {
            return _Context.MessageRooms.Any(x => x.Id == id);
        }

        public bool MessageRoomExists(string name)
        {
            return _Context.MessageRooms.Any(x => x.RoomName == name);
        }

        public bool RemoveMessageRoom(MessageRoom room)
        {
            _Context.Remove(room);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMessageRoom(MessageRoom room)
        {
            _Context.Update(room);
            return Save();
        }
    }
}
