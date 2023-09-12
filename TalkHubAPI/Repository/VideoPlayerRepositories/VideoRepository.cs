using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoRepository : IVideoRepository
    {
        public bool AddVideo(Video tag)
        {
            throw new NotImplementedException();
        }

        public Video GetVideo(int id)
        {
            throw new NotImplementedException();
        }

        public Video GetVideoByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<Video> GetVideos()
        {
            throw new NotImplementedException();
        }

        public ICollection<Video> GetVideosByPlaylistId(int playlistId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Video> GetVideosByTagId(int tagId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Video> GetVideosByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveVideo(Video tag)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateVideo(Video tag)
        {
            throw new NotImplementedException();
        }

        public bool VideoExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool VideoExists(string name)
        {
            throw new NotImplementedException();
        }
    }
}
