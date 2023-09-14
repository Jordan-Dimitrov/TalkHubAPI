using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoCommentsLikeRepository : IVideoCommentsLikeRepository
    {
        private readonly TalkHubContext _Context;
        public VideoCommentsLikeRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            _Context.Add(videoCommentsLike);
            return Save();
        }

        public VideoCommentsLike GetVideoCommentsLike(int id)
        {
            return _Context.VideoCommentsLikes.Find(id);
        }

        public VideoCommentsLike GetVideoCommentsLikeByCommentAndUser(int commenteId, int userId)
        {
            return _Context.VideoCommentsLikes
                .Where(x => x.VideoCommentId == commenteId && x.UserId == userId)
                .FirstOrDefault();
        }

        public ICollection<VideoCommentsLike> GetVideoCommentsLikes()
        {
            return _Context.VideoCommentsLikes.ToList();
        }

        public bool RemoveVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            _Context.Remove(videoCommentsLike);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideoCommentsLike(VideoCommentsLike videoCommentsLike)
        {
            _Context.Update(videoCommentsLike);
            return Save();
        }

        public bool VideoCommentsLikeExists(int id)
        {
            return _Context.VideoCommentsLikes.Any(x => x.Id == id);
        }

        public bool VideoCommentsLikeExistsForCommentAndUser(int commentId, int userId)
        {
            return _Context.VideoCommentsLikes.Any(x => x.VideoCommentId == commentId && x.UserId == userId);
        }
    }
}
