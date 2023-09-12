using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoTagRepository
    {
        bool AddVideoTag(VideoTag tag);
        bool RemoveVideoTag(VideoTag tag);
        bool UpdateVideoTag(VideoTag tag);
        bool VideoTagExists(int id);
        bool Save();
        VideoTag GetVideoTag(int id);
        ICollection<VideoTag> GetVideoTags();
        bool VideoTagExists(string name);
        VideoTag GetVideoTagByName(string name);
    }
}
