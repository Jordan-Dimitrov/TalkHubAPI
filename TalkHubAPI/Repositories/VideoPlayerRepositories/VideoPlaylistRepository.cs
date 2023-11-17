using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class VideoPlaylistRepository : IVideoPlaylistRepository
    {
        private readonly TalkHubContext _Context;

        public VideoPlaylistRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoPlaylistAsync(VideoPlaylist videoPlaylist)
        {
            await _Context.AddAsync(videoPlaylist);
            return await SaveAsync();
        }

        public async Task<VideoPlaylist?> GetVideoPlaylistAsync(int id)
        {
            return await _Context.VideoPlaylists.FindAsync(id);
        }

        public async Task<VideoPlaylist?> GetVideoPlaylistByVideoIdAndPlaylistIdAsync(int videoId, int playlistId)
        {
            return await _Context.VideoPlaylists
                .Where(x => x.PlaylistId == playlistId && x.VideoId == videoId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<VideoPlaylist>> GetVideoPlaylistsAsync()
        {
            return await _Context.VideoPlaylists.ToListAsync();
        }

        public async Task<ICollection<VideoPlaylist>> GetVideoPlaylistsForPlaylistAsync(int playlistId)
        {
            return await _Context.VideoPlaylists.Where(x => x.PlaylistId == playlistId).ToListAsync();
        }

        public async Task<bool> RemoveVideoPlaylistAsync(VideoPlaylist videoPlaylist)
        {
            _Context.Remove(videoPlaylist);
            return await SaveAsync();
        }

        public async Task<bool> RemoveVideoPlaylistsForPlaylistIdAsync(int playlistId)
        {
            _Context.RemoveRange(await GetVideoPlaylistsForPlaylistAsync(playlistId));
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoPlaylistAsync(VideoPlaylist videoPlaylist)
        {
            _Context.Update(videoPlaylist);
            return await SaveAsync();
        }

        public async Task<bool> VideoPlaylistExistsAsync(int id)
        {
            return await _Context.VideoPlaylists.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoPlaylistExistsForVideoAndPlaylistAsync(int videoId, int playlistId)
        {
            return await _Context.VideoPlaylists.AnyAsync(x => x.VideoId == videoId && x.PlaylistId == playlistId);
        }
    }
}
