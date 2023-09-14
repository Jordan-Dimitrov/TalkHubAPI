using TalkHubAPI.Models.VideoPlayerModels;
namespace TalkHubAPI.Interfaces.VideoPlayerInterfaces
{
    public interface IVideoPlaylistRepository
    {
        bool AddVideoPlaylist(VideoPlaylist videoPlaylist);
        bool RemoveVideoPlaylist(VideoPlaylist videoPlaylist);
        bool UpdateVideoPlaylist(VideoPlaylist videoPlaylist);
        bool VideoPlaylistExists(int id);
        bool Save();
        VideoPlaylist GetVideoPlaylist(int id);
        VideoPlaylist GetVideoPlaylistByVideoIdAndPlaylistId(int videoId, int playlistId);
        bool VideoPlaylistExistsForVideoAndPlaylist(int videoId, int playlistId);
        ICollection<VideoPlaylist> GetVideoPlaylists();
    }
}
