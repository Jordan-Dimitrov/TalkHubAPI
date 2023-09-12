using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IPlaylistRepository
    {
        bool AddPlaylist(Playlist playlist);
        bool RemovePlaylist(Playlist playlist);
        bool UpdatePlaylist(Playlist playlist);
        bool PlaylistExists(int id);
        bool Save();
        Playlist GetPlaylist(int id);
        ICollection<Playlist> GetPlaylists();
        ICollection<Playlist> GetPlaylistsByUserId(int userId);
        bool PlaylistExists(string name);
        Playlist GetPlaylistByName(string name);
    }
}
