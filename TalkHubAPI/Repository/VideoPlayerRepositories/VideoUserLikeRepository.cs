using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoUserLikeRepository : IVideoUserLikeRepository
    {
        private readonly TalkHubContext _Context;

        public VideoUserLikeRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            _Context.Add(videoUserLike);
            return await SaveAsync();
        }

        public async Task<VideoUserLike> GetVideoUserLikeAsync(int id)
        {
            return await _Context.VideoUserLikes.FindAsync(id);
        }

        public async Task<VideoUserLike> GetVideoUserLikeByVideoAndUserAsync(int videoId, int userId)
        {
            return await _Context.VideoUserLikes
                .Where(x => x.VideoId == videoId && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<VideoUserLike>> GetVideoUserLikesAsync()
        {
            return await _Context.VideoUserLikes.ToListAsync();
        }

        public async Task<bool> RemoveVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            _Context.Remove(videoUserLike);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            _Context.Update(videoUserLike);
            return await SaveAsync();
        }

        public async Task<bool> VideoUserLikeExistsAsync(int id)
        {
            return await _Context.VideoUserLikes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoUserLikeExistsForVideoAndUserAsync(int videoId, int userId)
        {
            return await _Context.VideoUserLikes.AnyAsync(x => x.VideoId == videoId && x.UserId == userId);
        }
    }
}
