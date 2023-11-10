using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoCommentsLikeRepository
    {
        Task<bool> AddVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike);
        Task<bool> RemoveVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike);
        Task<bool> UpdateVideoCommentsLikeAsync(VideoCommentsLike videoCommentsLike);
        Task<bool> VideoCommentsLikeExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<bool> VideoCommentsLikeExistsForCommentAndUserAsync(int commentId, int userId);
        Task<VideoCommentsLike?> GetVideoCommentsLikeByCommentAndUserAsync(int commentId, int userId);
        Task<VideoCommentsLike?> GetVideoCommentsLikeAsync(int id);
        Task<ICollection<VideoCommentsLike>> GetVideoCommentsLikesAsync();
    }
}
