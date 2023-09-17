﻿using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Repository.ForumRepositories
{
    public class ForumMessageRepository : IForumMessageRepository
    {
        private readonly TalkHubContext _Context;

        public ForumMessageRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddForumMessageAsync(ForumMessage message)
        {
            _Context.Add(message);
            return await SaveAsync();
        }

        public async Task<bool> ForumMessageExistsAsync(int id)
        {
            return await _Context.ForumMessages.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ForumMessageExistsAsync(string name)
        {
            return await _Context.ForumMessages.AnyAsync(x => x.MessageContent == name);
        }

        public async Task<ForumMessage> GetForumMessageAsync(int id)
        {
            return await _Context.ForumMessages.FindAsync(id);
        }

        public async Task<ForumMessage> GetForumMessageByNameAsync(string name)
        {
            return await _Context.ForumMessages.FirstOrDefaultAsync(x => x.MessageContent == name);
        }

        public async Task<IList<ForumMessage>> GetForumMessagesAsync()
        {
            return await _Context.ForumMessages.ToListAsync();
        }

        public async Task<IList<ForumMessage>> GetForumMessagesByForumThreadIdAsync(int forumThreadId)
        {
            return await _Context.ForumMessages
                .Where(x => x.ForumThreadId == forumThreadId)
                .ToListAsync();
        }

        public async Task<IList<ForumMessage>> GetForumMessagesByUserIdAsync(int userId)
        {
            return await _Context.ForumMessages
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveForumMessageAsync(ForumMessage message)
        {
            _Context.Remove(message);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateForumMessageAsync(ForumMessage message)
        {
            _Context.Update(message);
            return await SaveAsync();
        }
    }
}
