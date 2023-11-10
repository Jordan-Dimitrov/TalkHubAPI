using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoPlaylistRepository
    {
        Task<bool> AddVideoPlaylistAsync(VideoPlaylist videoPlaylist);
        Task<bool> RemoveVideoPlaylistAsync(VideoPlaylist videoPlaylist);
        Task<bool> UpdateVideoPlaylistAsync(VideoPlaylist videoPlaylist);
        Task<bool> VideoPlaylistExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<VideoPlaylist?> GetVideoPlaylistAsync(int id);
        Task<VideoPlaylist?> GetVideoPlaylistByVideoIdAndPlaylistIdAsync(int videoId, int playlistId);
        Task<bool> VideoPlaylistExistsForVideoAndPlaylistAsync(int videoId, int playlistId);
        Task<ICollection<VideoPlaylist>> GetVideoPlaylistsAsync();
    }
}
