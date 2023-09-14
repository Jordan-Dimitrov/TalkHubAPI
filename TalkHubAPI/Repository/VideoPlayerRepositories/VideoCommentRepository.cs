using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoCommentRepository : IVideoCommentRepository
    {
        private readonly TalkHubContext _Context;
        public VideoCommentRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideoComment(VideoComment message)
        {
            _Context.Add(message);
            return Save();
        }

        public VideoComment GetVideoComment(int id)
        {
            return _Context.VideoComments.Find(id);
        }

        public VideoComment GetVideoCommentByName(string name)
        {
            return _Context.VideoComments.FirstOrDefault(x => x.MessageContent == name);
        }

        public ICollection<VideoComment> GetVideoComments()
        {
            return _Context.VideoComments.ToList();
        }

        public ICollection<VideoComment> GetVideoCommentsByUserId(int userId)
        {
            return _Context.VideoComments.Where(x => x.UserId == userId).ToList();
        }

        public ICollection<VideoComment> GetVideoCommentsByVideoId(int videoId)
        {
            return _Context.VideoComments.Where(x => x.VideoId == videoId).ToList();
        }

        public bool RemoveVideoComment(VideoComment message)
        {
            _Context.Remove(message);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideoComment(VideoComment message)
        {
            _Context.Update(message);
            return Save();
        }

        public bool VideoCommentExists(int id)
        {
            return _Context.VideoComments.Any(x => x.Id == id);
        }

        public bool VideoCommentExists(string name)
        {
            return _Context.VideoComments.Any(x => x.MessageContent == name);
        }
    }
}
