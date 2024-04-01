using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class VideoTagRepository : IVideoTagRepository
    {
        private readonly TalkHubContext _Context;
        public VideoTagRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoTagAsync(VideoTag tag)
        {
            await _Context.AddAsync(tag);
            return await SaveAsync();
        }

        public async Task<VideoTag?> GetVideoTagAsync(int id)
        {
            return await _Context.VideoTags.FindAsync(id);
        }

        public async Task<VideoTag?> GetVideoTagByNameAsync(string name)
        {
            return await _Context.VideoTags.FirstOrDefaultAsync(x => x.TagName == name);
        }

        public async Task<ICollection<VideoTag>> GetVideoTagsAsync()
        {
            return await _Context.VideoTags.ToListAsync();
        }

        public async Task<bool> RemoveVideoTagAsync(VideoTag tag)
        {
            _Context.Remove(tag);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoTagAsync(VideoTag tag)
        {
            _Context.Update(tag);
            return await SaveAsync();
        }

        public async Task<bool> VideoTagExistsAsync(int id)
        {
            return await _Context.VideoTags.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoTagExistsAsync(string name)
        {
            return await _Context.VideoTags.AnyAsync(x => x.TagName == name);
        }
    }
}
