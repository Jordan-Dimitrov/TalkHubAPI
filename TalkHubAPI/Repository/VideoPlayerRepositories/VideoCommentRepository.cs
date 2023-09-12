using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoCommentRepository : IVideoCommentRepository
    {
        public bool AddVideoComment(VideoComment message)
        {
            throw new NotImplementedException();
        }

        public VideoComment GetVideoComment(int id)
        {
            throw new NotImplementedException();
        }

        public VideoComment GetVideoCommentByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<VideoComment> GetVideoComments()
        {
            throw new NotImplementedException();
        }

        public ICollection<VideoComment> GetVideoCommentsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public ICollection<VideoComment> GetVideoCommentsByVideoId(int videoId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveVideoComment(VideoComment message)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateVideoComment(VideoComment message)
        {
            throw new NotImplementedException();
        }

        public bool VideoCommentExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool VideoCommentExists(string name)
        {
            throw new NotImplementedException();
        }
    }
}
