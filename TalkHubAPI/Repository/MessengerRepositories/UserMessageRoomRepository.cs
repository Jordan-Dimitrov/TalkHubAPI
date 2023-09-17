using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddUserMessageRoomAsync(UserMessageRoom userMessageRoom)
        {
            _Context.Add(userMessageRoom);
            return await SaveAsync();
        }

        public async Task<UserMessageRoom> GetUserMessageRoomAsync(int id)
        {
            return await _Context.UserMessageRooms.FindAsync(id);
        }

        public async Task<ICollection<UserMessageRoom>> GetUserMessageRoomsAsync()
        {
            return await _Context.UserMessageRooms.ToListAsync();
        }

        public async Task<bool> RemoveUserMessageRoomAsync(UserMessageRoom userMessageRoom)
        {
            _Context.Remove(userMessageRoom);
            return await SaveAsync();
        }

        public async Task<bool> UpdateUserMessageRoomAsync(UserMessageRoom userMessageRoom)
        {
            _Context.Update(userMessageRoom);
            return await SaveAsync();
        }

        public async Task<bool> UserMessageRoomExistsAsync(int id)
        {
            return await _Context.UserMessageRooms.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> UserMessageRoomExistsForRoomAndUserAsync(int roomId, int userId)
        {
            return await _Context.UserMessageRooms.AnyAsync(x => x.RoomId == roomId && x.UserId == userId);
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<ICollection<UserMessageRoom>> GetUserMessageRoomsAsyncForRoom(int roomId)
        {
            return await _Context.UserMessageRooms.Where(x => x.RoomId == roomId).ToListAsync();
        }

        public async Task<bool> RemoveUserMessageRoomForRoomId(int roomId)
        {
            _Context.RemoveRange(await GetUserMessageRoomsAsyncForRoom(roomId));
            return await SaveAsync();
        }
    }
}
