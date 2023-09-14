using System.ComponentModel.Design;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoUserLikeRepository : IVideoUserLikeRepository
    {
        private readonly TalkHubContext _Context;
        public VideoUserLikeRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideoUserLike(VideoUserLike videoUserLike)
        {
            _Context.Add(videoUserLike);
            return Save();
        }

        public VideoUserLike GetVideoUserLike(int id)
        {
            return _Context.VideoUserLikes.Find(id);
        }

        public VideoUserLike GetVideoUserLikeByVideoAndUser(int videoId, int userId)
        {
            return _Context.VideoUserLikes
                .Where(x => x.VideoId == videoId && x.UserId == userId)
                .FirstOrDefault();
        }

        public ICollection<VideoUserLike> GetVideUserLikes()
        {
            return _Context.VideoUserLikes.ToList();
        }

        public bool RemoveVideoUserLike(VideoUserLike videoUserLike)
        {
            _Context.Remove(videoUserLike);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideoUserLike(VideoUserLike videoUserLike)
        {
            _Context.Update(videoUserLike);
            return Save();
        }

        public bool VideoUserLikeExists(int id)
        {
            return _Context.VideoUserLikes.Any(x => x.Id == id);
        }

        public bool VideoUserLikeExistsForVideoAndUser(int videoId, int userId)
        {
            return _Context.VideoUserLikes.Any(x => x.VideoId == videoId && x.UserId == userId);
        }
    }
}
