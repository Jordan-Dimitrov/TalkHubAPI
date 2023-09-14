using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly TalkHubContext _Context;

        public PlaylistRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddPlaylist(Playlist playlist)
        {
            _Context.Add(playlist);
            return Save();
        }

        public Playlist GetPlaylist(int id)
        {
            return _Context.Playlists.Find(id);
        }

        public Playlist GetPlaylistByName(string name)
        {
            return _Context.Playlists.FirstOrDefault(x => x.PlaylistName == name);
        }

        public ICollection<Playlist> GetPlaylists()
        {
            return _Context.Playlists.ToList();
        }

        public ICollection<Playlist> GetPlaylistsByUserId(int userId)
        {
            return _Context.Playlists.Where(x => x.UserId == userId).ToList();
        }

        public bool PlaylistExists(int id)
        {
            return _Context.Playlists.Any(x => x.Id == id);
        }

        public bool PlaylistExists(string name)
        {
            return _Context.Playlists.Any(x => x.PlaylistName == name);
        }

        public bool RemovePlaylist(Playlist playlist)
        {
            _Context.Remove(playlist);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePlaylist(Playlist playlist)
        {
            _Context.Update(playlist);
            return Save();
        }
    }
}
