using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoUserLikeRepository
    {
        bool AddVideoUserLike(VideoUserLike videoUserLike);
        bool RemoveVideoUserLike(VideoUserLike videoUserLike);
        bool UpdateVideoUserLike(VideoUserLike videoUserLike);
        bool VideoUserLikeExists(int id);
        bool Save();
        bool VideoUserLikeExistsForVideoAndUser(int videoId, int userId);
        VideoUserLike GetVideoUserLikeByVideoAndUser(int videoId, int userId);
        VideoUserLike GetVideoUserLike(int id);
        ICollection<VideoUserLike> GetVideUserLikes();
    }
}
