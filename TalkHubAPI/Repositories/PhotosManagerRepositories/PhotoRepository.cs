using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Repositories.PhotosManagerRepositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly TalkHubContext _Context;

        public PhotoRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public async Task<bool> AddPhotoAsync(Photo photo)
        {
            await _Context.AddAsync(photo);
            return await SaveAsync();
        }

        public async Task<Photo?> GetPhotoAsync(int id)
        {
            return await _Context.Photos.FindAsync(id);
        }

        public async Task<ICollection<Photo>> GetPhotosAsync()
        {
            return await _Context.Photos.ToListAsync();
        }

        public async Task<ICollection<Photo>> GetPhotosByCategoryIdAsync(int categoryId)
        {
            return await _Context.Photos.Where(x => x.CategoryId == categoryId).ToListAsync();
        }

        public async Task<ICollection<Photo>> GetPhotosByUserIdAsync(int userId)
        {
            return await _Context.Photos.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> PhotoExistsAsync(int id)
        {
            return await _Context.Photos.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> RemovePhotoAsync(Photo photo)
        {
            _Context.Remove(photo);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdatePhotoAsync(Photo photo)
        {
            _Context.Update(photo);
            return await SaveAsync();
        }
    }
}
