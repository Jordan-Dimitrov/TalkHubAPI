﻿using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Repositories.VideoPlayerRepositories
{
    public class VideoUserLikeRepository : IVideoUserLikeRepository
    {
        private readonly TalkHubContext _Context;

        public VideoUserLikeRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            await _Context.AddAsync(videoUserLike);
            return await SaveAsync();
        }

        public async Task<VideoUserLike?> GetVideoUserLikeAsync(int id)
        {
            return await _Context.VideoUserLikes.Include(x => x.Video).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VideoUserLike?> GetVideoUserLikeByVideoAndUserAsync(int videoId, int userId)
        {
            return await _Context.VideoUserLikes
                .Include(x => x.Video).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.VideoId == videoId && x.UserId == userId);
        }

        public async Task<ICollection<VideoUserLike>> GetVideoUserLikesAsync()
        {
            return await _Context.VideoUserLikes.ToListAsync();
        }

        public async Task<bool> RemoveVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            _Context.Remove(videoUserLike);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateVideoUserLikeAsync(VideoUserLike videoUserLike)
        {
            _Context.Update(videoUserLike);
            return await SaveAsync();
        }

        public async Task<bool> VideoUserLikeExistsAsync(int id)
        {
            return await _Context.VideoUserLikes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VideoUserLikeExistsForVideoAndUserAsync(int videoId, int userId)
        {
            return await _Context.VideoUserLikes.AnyAsync(x => x.VideoId == videoId && x.UserId == userId);
        }
    }
}
