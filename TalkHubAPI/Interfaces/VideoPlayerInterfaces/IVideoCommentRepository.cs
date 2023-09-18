using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoCommentRepository
    {
        Task<bool> AddVideoCommentAsync(VideoComment comment);
        Task<bool> RemoveVideoCommentAsync(VideoComment comment);
        Task<bool> UpdateVideoCommentAsync(VideoComment comment);
        Task<bool> VideoCommentExistsAsync(int id);
        Task<bool> VideoCommentExistsAsync(string name);
        Task<bool> SaveAsync();
        Task<VideoComment> GetVideoCommentAsync(int id);
        Task<VideoComment> GetVideoCommentByNameAsync(string name);
        Task<ICollection<VideoComment>> GetVideoCommentsAsync();
        Task<ICollection<VideoComment>> GetVideoCommentsByVideoIdAsync(int videoId);
        Task<ICollection<VideoComment>> GetVideoCommentsByUserIdAsync(int userId);
    }
}
