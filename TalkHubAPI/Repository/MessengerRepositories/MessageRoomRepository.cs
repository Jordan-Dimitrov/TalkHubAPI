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
        public async Task<bool> AddMessageRoomAsync(MessageRoom room)
        {
            _Context.Add(room);
            return await SaveAsync();
        }

        public async Task<MessageRoom> GetMessageRoomAsync(int id)
        {
            return await _Context.MessageRooms.FindAsync(id);
        }

        public async Task<MessageRoom> GetMessageRoomByNameAsync(string name)
        {
            return await _Context.MessageRooms.FirstOrDefaultAsync(x => x.RoomName == name);
        }

        public async Task<List<MessageRoom>> GetMessageRoomsAsync()
        {
            return await _Context.MessageRooms.ToListAsync();
        }

        public async Task<List<MessageRoom>> GetMessageRoomsForUserAsync(int userId)
        {
            return await _Context.MessageRooms
                .Where(x => x.UserMessageRooms.Any(a => a.UserId == userId))
                .Include(x => x.MessengerMessages)
                .ToListAsync();
        }

        public async Task<bool> MessageRoomExistsAsync(int id)
        {
            return await _Context.MessageRooms.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> MessageRoomExistsAsync(string name)
        {
            return await _Context.MessageRooms.AnyAsync(x => x.RoomName == name);
        }

        public async Task<bool> RemoveMessageRoomAsync(MessageRoom room)
        {
            _Context.Remove(room);
            return await SaveAsync();
        }

        public async Task<bool> UpdateMessageRoomAsync(MessageRoom room)
        {
            _Context.Update(room);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
