using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoUserLikeRepository
    {
        Task<bool> AddVideoUserLikeAsync(VideoUserLike videoUserLike);
        Task<bool> RemoveVideoUserLikeAsync(VideoUserLike videoUserLike);
        Task<bool> UpdateVideoUserLikeAsync(VideoUserLike videoUserLike);
        Task<bool> VideoUserLikeExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<bool> VideoUserLikeExistsForVideoAndUserAsync(int videoId, int userId);
        Task<VideoUserLike> GetVideoUserLikeByVideoAndUserAsync(int videoId, int userId);
        Task<VideoUserLike> GetVideoUserLikeAsync(int id);
        Task<ICollection<VideoUserLike>> GetVideoUserLikesAsync();
    }
}
