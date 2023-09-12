using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoCommentsLikeRepository
    {
        bool AddVideoCommentsLike(VideoCommentsLike videoCommentsLike);
        bool RemoveVideoCommentsLike(VideoCommentsLike videoCommentsLike);
        bool UpdateVideoCommentsLike(VideoCommentsLike videoCommentsLike);
        bool VideoCommentsLikeExists(int id);
        bool Save();
        bool VideoCommentsLikeExistsForCommentAndUser(int commentId, int userId);
        VideoCommentsLike GetVideoCommentsLikeByCommentAndUser(int commenteId, int userId);
        VideoCommentsLike GetVideoCommentsLike(int id);
        ICollection<VideoCommentsLike> GetVideoCommentsLikes();
    }
}
