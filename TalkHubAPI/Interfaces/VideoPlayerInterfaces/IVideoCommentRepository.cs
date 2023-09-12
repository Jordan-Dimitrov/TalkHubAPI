using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoCommentRepository
    {
        bool AddVideoComment(VideoComment message);
        bool RemoveVideoComment(VideoComment message);
        bool UpdateVideoComment(VideoComment message);
        bool VideoCommentExists(int id);
        bool Save();
        VideoComment GetVideoComment(int id);
        bool VideoCommentExists(string name);
        VideoComment GetVideoCommentByName(string name);
        ICollection<VideoComment> GetVideoComments();
        ICollection<VideoComment> GetVideoCommentsByVideoId(int videoId);
        ICollection<VideoComment> GetVideoCommentsByUserId(int userId);
    }
}
