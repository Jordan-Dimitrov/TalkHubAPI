using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repository.VideoPlayerRepositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        public bool AddPlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public Playlist GetPlaylist(int id)
        {
            throw new NotImplementedException();
        }

        public Playlist GetPlaylistByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<Playlist> GetPlaylists()
        {
            throw new NotImplementedException();
        }

        public ICollection<Playlist> GetPlaylistsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public bool PlaylistExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool PlaylistExists(string name)
        {
            throw new NotImplementedException();
        }

        public bool RemovePlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdatePlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }
    }
}
