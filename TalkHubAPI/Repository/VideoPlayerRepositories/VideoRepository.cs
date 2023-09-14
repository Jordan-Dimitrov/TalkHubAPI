using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly TalkHubContext _Context;
        public VideoRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideo(Video video)
        {
            _Context.Add(video);
            return Save();
        }

        public Video GetVideo(int id)
        {
            return _Context.Videos.Find(id);
        }

        public Video GetVideoByName(string name)
        {
            return _Context.Videos.FirstOrDefault(x => x.VideoName == name);
        }

        public ICollection<Video> GetVideos()
        {
            return _Context.Videos.ToList();
        }

        public ICollection<Video> GetVideosByPlaylistId(int playlistId)
        {
            return _Context.Videos
                .Where(x => x.VideoPlaylists.Any(a => a.PlaylistId == playlistId))
                .Include(x => x.User)
                .Include(x => x.Tag)
                .ToList();
        }

        public ICollection<Video> GetVideosByTagId(int tagId)
        {
            return _Context.Videos.Where(x => x.TagId == tagId).ToList();
        }

        public ICollection<Video> GetVideosByUserId(int userId)
        {
            return _Context.Videos.Where(x => x.UserId == userId).ToList();
        }

        public bool RemoveVideo(Video video)
        {
            _Context.Remove(video);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideo(Video video)
        {
            _Context.Update(video);
            return Save();
        }

        public bool VideoExists(int id)
        {
            return _Context.Videos.Any(x => x.Id == id);
        }

        public bool VideoExists(string name)
        {
            return _Context.Videos.Any(x => x.VideoName == name);
        }
    }
}
