using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class VideoCommentRepository : IVideoCommentRepository
    {
        private readonly TalkHubContext _Context;

        public VideoCommentRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoCommentAsync(VideoComment comment)
        {
            await _Context.AddAsync(comment);
            return await SaveAsync();
        }

        public async Task<VideoComment?> GetVideoCommentAsync(int id)
        {
            return await _Context.VideoComments.FindAsync(id);
        }

        public async Task<VideoComment?> GetVideoCommentByNameAsync(string name)
        {
            return await _Context.VideoComments.FirstOrDefaultAsync(x => x.MessageContent == name);
        }

        public async Task<ICollection<VideoComment>> GetVideoCommentsAsync()
        {
            return await _Context.VideoComments.ToListAsync();
        }

        public async Task<ICollection<VideoComment>> GetVideoCommentsByUserIdAsync(int userId)
        {
            return await _Context.VideoComments.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<VideoComment>> GetVideoCommentsByVideoIdAsync(int videoId)
        {
            return await _Context.VideoComments.Where(x => x.VideoId == videoId).ToListAsync();
        }

        public async Task<bool> RemoveVideoCommentAsync(VideoComment comment)
        {
            _Context.Remove(comment);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoCommentAsync(VideoComment comment)
        {
            _Context.Update(comment);
            return await SaveAsync();
        }

        public async Task<bool> VideoCommentExistsAsync(int id)
        {
            return await _Context.VideoComments.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoCommentExistsAsync(string name)
        {
            return await _Context.VideoComments.AnyAsync(x => x.MessageContent == name);
        }
    }
}
