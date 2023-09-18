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

        public async Task<bool> AddVideoAsync(Video video)
        {
            await _Context.AddAsync(video);
            return await SaveAsync();
        }

        public async Task<Video> GetVideoAsync(int id)
        {
            return await _Context.Videos.FindAsync(id);
        }

        public async Task<Video> GetVideoByNameAsync(string name)
        {
            return await _Context.Videos.FirstOrDefaultAsync(x => x.VideoName == name);
        }

        public async Task<ICollection<Video>> GetVideosAsync()
        {
            return await _Context.Videos.ToListAsync();
        }

        public async Task<ICollection<Video>> GetVideosByPlaylistIdAsync(int playlistId)
        {
            return await _Context.Videos
                .Where(x => x.VideoPlaylists.Any(a => a.PlaylistId == playlistId))
                .Include(x => x.User)
                .Include(x => x.Tag)
                .ToListAsync();
        }

        public async Task<ICollection<Video>> GetVideosByTagIdAsync(int tagId)
        {
            return await _Context.Videos.Where(x => x.TagId == tagId).ToListAsync();
        }

        public async Task<ICollection<Video>> GetVideosByUserIdAsync(int userId)
        {
            return await _Context.Videos.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> RemoveVideoAsync(Video video)
        {
            _Context.Remove(video);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoAsync(Video video)
        {
            _Context.Update(video);
            return await SaveAsync();
        }

        public async Task<bool> VideoExistsAsync(int id)
        {
            return await _Context.Videos.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoExistsAsync(string name)
        {
            return await _Context.Videos.AnyAsync(x => x.VideoName == name);
        }
    }
}
