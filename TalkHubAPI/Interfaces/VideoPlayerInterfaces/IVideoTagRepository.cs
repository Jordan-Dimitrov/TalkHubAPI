using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoTagRepository
    {
        Task<bool> AddVideoTagAsync(VideoTag tag);
        Task<bool> RemoveVideoTagAsync(VideoTag tag);
        Task<bool> UpdateVideoTagAsync(VideoTag tag);
        Task<bool> VideoTagExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<VideoTag> GetVideoTagAsync(int id);
        Task<ICollection<VideoTag>> GetVideoTagsAsync();
        Task<bool> VideoTagExistsAsync(string name);
        Task<VideoTag> GetVideoTagByNameAsync(string name);
    }
}
