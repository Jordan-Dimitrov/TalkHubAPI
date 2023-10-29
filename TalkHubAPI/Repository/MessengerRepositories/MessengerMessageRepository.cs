using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> AddMessengerMessageAsync(MessengerMessage message)
        {
            await _Context.AddAsync(message);
            return await SaveAsync();
        }

        public async Task<MessengerMessage> GetMessengerMessageAsync(int id)
        {
            return await _Context.MessengerMessages.FindAsync(id);
        }

        public async Task<ICollection<MessengerMessage>> GetMessengerMessagesAsync()
        {
            return await _Context.MessengerMessages.ToListAsync();
        }

        public async Task<ICollection<MessengerMessage>> GetLastTenMessengerMessagesFromLastMessageIdAsync(int messageId, int roomId)
        {
            return await _Context.MessengerMessages
                .Where(x => x.Id < messageId && x.RoomId == roomId)
                .OrderBy(x => x.Id)
                .Take(_MessagesToRetrieve)
                .ToListAsync();
        }

        public async Task<ICollection<MessengerMessage>> GetMessengerMessagesByRoomIdAsync(int roomId)
        {
            return await _Context.MessengerMessages
                .Where(x => x.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<ICollection<MessengerMessage>> GetMessengerMessagesByUserIdAsync(int userId)
        {
            return await _Context.MessengerMessages
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveMessengerMessageAsync(MessengerMessage message)
        {
            _Context.Remove(message);
            return await SaveAsync();
        }

        public async Task<bool> UpdateMessengerMessageAsync(MessengerMessage message)
        {
            _Context.Update(message);
            return await SaveAsync();
        }

        public async Task<bool> MessengerMessageExistsAsync(int id)
        {
            return await _Context.MessengerMessages.AnyAsync(x => x.Id == id);
        }

        public async Task<MessengerMessage> GetLastMessageAsync()
        {
            return await _Context.MessengerMessages
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
