using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Repository.MessengerRepositories
{
    public class UserMessageRoomRepository : IUserMessageRoomRepository
    {
        private readonly TalkHubContext _Context;

        public UserMessageRoomRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddUserMessageRoom(UserMessageRoom userMessageRoom)
        {
            _Context.Add(userMessageRoom);
            return Save();
        }

        public UserMessageRoom GetUserMessageRoom(int id)
        {
            return _Context.UserMessageRooms.Find(id);
        }

        public ICollection<UserMessageRoom> GetUserMessageRooms()
        {
            return _Context.UserMessageRooms.ToList();
        }

        public bool RemoveUserMessageRoom(UserMessageRoom userMessageRoom)
        {
            _Context.Remove(userMessageRoom);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserMessageRoom(UserMessageRoom userMessageRoom)
        {
            _Context.Update(userMessageRoom);
            return Save();
        }

        public bool UserMessageRoomExists(int id)
        {
            return _Context.UserMessageRooms.Any(x => x.Id == id);
        }

        public bool UserMessageRoomExistsForRoomAndUser(int roomId, int userId)
        {
            return _Context.UserMessageRooms.Any(x => x.RoomId == roomId && x.UserId == userId);
        }
    }
}
