using System.ComponentModel.Design;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class VideoPlaylistRepository : IVideoPlaylistRepository
    {
        private readonly TalkHubContext _Context;
        public VideoPlaylistRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddVideoPlaylist(VideoPlaylist videoPlaylist)
        {
            _Context.Add(videoPlaylist);
            return Save();
        }

        public VideoPlaylist GetVideoPlaylist(int id)
        {
            return _Context.VideoPlaylists.Find(id);
        }

        public VideoPlaylist GetVideoPlaylistByVideoIdAndPlaylistId(int videoId, int playlistId)
        {
            return _Context.VideoPlaylists
                            .Where(x => x.PlaylistId == playlistId && x.VideoId == videoId)
                            .FirstOrDefault();
        }

        public ICollection<VideoPlaylist> GetVideoPlaylists()
        {
            return _Context.VideoPlaylists.ToList();
        }

        public bool RemoveVideoPlaylist(VideoPlaylist videoPlaylist)
        {
            _Context.Remove(videoPlaylist);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVideoPlaylist(VideoPlaylist videoPlaylist)
        {
            _Context.Update(videoPlaylist);
            return Save();
        }

        public bool VideoPlaylistExists(int id)
        {
            return _Context.VideoPlaylists.Any(x => x.Id == id);
        }

        public bool VideoPlaylistExistsForVideoAndPlaylist(int videoId, int playlistId)
        {
            return _Context.VideoPlaylists.Any(x => x.VideoId == videoId && x.PlaylistId == playlistId);
        }
    }
}
