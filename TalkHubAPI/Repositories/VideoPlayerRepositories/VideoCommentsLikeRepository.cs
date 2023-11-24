using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class VideoCommentsLikeRepository : IVideoCommentsLikeRepository
    {
        private readonly TalkHubContext _Context;

        public VideoCommentsLikeRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike)
        {
            await _Context.AddAsync(videoCommentsLike);
            return await SaveAsync();
        }

        public async Task<VideoCommentsLike?> GetVideoCommentsLikeAsync(int id)
        {
            return await _Context.VideoCommentsLikes.Include(x => x.User).Include(x => x.VideoComment)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VideoCommentsLike?> GetVideoCommentsLikeByCommentAndUserAsync(int commentId, int userId)
        {
            return await _Context.VideoCommentsLikes.Include(x => x.User).Include(x => x.VideoComment)
                .FirstOrDefaultAsync(x => x.VideoCommentId == commentId && x.UserId == userId);
        }

        public async Task<ICollection<VideoCommentsLike>> GetVideoCommentsLikesAsync()
        {
            return await _Context.VideoCommentsLikes.ToListAsync();
        }

        public async Task<bool> RemoveVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike)
        {
            _Context.Remove(videoCommentsLike);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike)
        {
            _Context.Update(videoCommentsLike);
            return await SaveAsync();
        }

        public async Task<bool> VideoCommentsLikeExistsAsync(int id)
        {
            return await _Context.VideoCommentsLikes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoCommentsLikeExistsForCommentAndUserAsync(int commentId, int userId)
        {
            return await _Context.VideoCommentsLikes.AnyAsync(x => x.VideoCommentId == commentId && x.UserId == userId);
        }
    }
}
