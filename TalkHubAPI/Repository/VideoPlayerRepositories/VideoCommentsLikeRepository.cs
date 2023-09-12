using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoCommentsLikeRepository : IVideoCommentsLikeRepository
    {
        public bool AddVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            throw new NotImplementedException();
        }

        public VideoCommentsLike GetVideoCommentsLike(int id)
        {
            throw new NotImplementedException();
        }

        public VideoCommentsLike GetVideoCommentsLikeByCommentAndUser(int commenteId, int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<VideoCommentsLike> GetVideoCommentsLikes()
        {
            throw new NotImplementedException();
        }

        public bool RemoveVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            throw new NotImplementedException();
        }

        public bool VideoCommentsLikeExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool VideoCommentsLikeExistsForCommentAndUser(int commentId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
