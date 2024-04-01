using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoRepository
    {
        Task<bool> AddVideoAsync(Video video);
        Task<bool> RemoveVideoAsync(Video video);
        Task<bool> UpdateVideoAsync(Video video);
        Task<bool> VideoExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<Video?> GetVideoAsync(int id);
        Task<ICollection<Video>> GetVideosAsync();
        Task<bool> VideoExistsAsync(string name);
        Task<Video?> GetVideoByNameAsync(string name);
        Task<ICollection<Video>> GetVideosByTagIdAsync(int tagId);
        Task<ICollection<Video>> GetVideosByUserIdAsync(int userId);
        Task<ICollection<Video>> GetVideosByPlaylistIdAsync(int playlistId);
        Task<ICollection<Video>> GetRecommendedVideosByUserIdAsync(int userId);
        Task<ICollection<Video>> GetAllUserSubscribedChannelVideosAsync(int userId);
    }
}
