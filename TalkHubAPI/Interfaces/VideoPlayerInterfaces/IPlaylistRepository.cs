using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IPlaylistRepository
    {
        Task<bool> AddPlaylistAsync(Playlist playlist);
        Task<bool> RemovePlaylistAsync(Playlist playlist);
        Task<bool> UpdatePlaylistAsync(Playlist playlist);
        Task<bool> PlaylistExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<Playlist> GetPlaylistAsync(int id);
        Task<ICollection<Playlist>> GetPlaylistsAsync();
        Task<ICollection<Playlist>> GetPlaylistsByUserIdAsync(int userId);
        Task<bool> PlaylistExistsAsync(string name);
        Task<Playlist> GetPlaylistByNameAsync(string name);
    }
}
