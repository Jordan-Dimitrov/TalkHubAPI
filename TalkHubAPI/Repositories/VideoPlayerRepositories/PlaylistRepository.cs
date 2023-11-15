using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly TalkHubContext _Context;

        public PlaylistRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddPlaylistAsync(Playlist playlist)
        {
            await _Context.AddAsync(playlist);
            return await SaveAsync();
        }

        public async Task<Playlist?> GetPlaylistAsync(int id)
        {
            return await _Context.Playlists.FindAsync(id);
        }

        public async Task<Playlist?> GetPlaylistByNameAsync(string name)
        {
            return await _Context.Playlists.FirstOrDefaultAsync(x => x.PlaylistName == name);
        }

        public async Task<ICollection<Playlist>> GetPlaylistsAsync()
        {
            return await _Context.Playlists.ToListAsync();
        }

        public async Task<ICollection<Playlist>> GetPlaylistsByUserIdAsync(int userId)
        {
            return await _Context.Playlists.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> PlaylistExistsAsync(int id)
        {
            return await _Context.Playlists.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> PlaylistExistsAsync(string name)
        {
            return await _Context.Playlists.AnyAsync(x => x.PlaylistName == name);
        }

        public async Task<bool> PlaylistExistsForUserAsync(int userId)
        {
            return await _Context.Playlists.AnyAsync(x => x.UserId == userId);
        }

        public async Task<bool> RemovePlaylistAsync(Playlist playlist)
        {
            _Context.Remove(playlist);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdatePlaylistAsync(Playlist playlist)
        {
            _Context.Update(playlist);
            return await SaveAsync();
        }
    }
}
