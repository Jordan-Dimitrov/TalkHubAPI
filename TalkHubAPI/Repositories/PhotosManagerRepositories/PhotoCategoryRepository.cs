using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Repositories.PhotosManagerRepositories
{
    public class PhotoCategoryRepository : IPhotoCategoryRepository
    {
        private readonly TalkHubContext _Context;

        public PhotoCategoryRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddCategoryAsync(PhotoCategory category)
        {
            await _Context.AddAsync(category);
            return await SaveAsync();
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _Context.PhotoCategories.AnyAsync(x => x.Id == id);
        }

        public async Task<ICollection<PhotoCategory>> GetCategoriesAsync()
        {
            return await _Context.PhotoCategories.ToListAsync();
        }

        public async Task<PhotoCategory?> GetCategoryAsync(int id)
        {
            return await _Context.PhotoCategories.FindAsync(id);
        }

        public async Task<bool> RemoveCategoryAsync(PhotoCategory category)
        {
            _Context.Remove(category);
            return await SaveAsync();
        }
        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<bool> UpdateCategoryAsync(PhotoCategory category)
        {
            _Context.Update(category);
            return await SaveAsync();
        }

        public async Task<bool> PhotoCategoryExistsAsync(string name)
        {
            return await _Context.PhotoCategories.AnyAsync(x => x.CategoryName == name);
        }

        public async Task<PhotoCategory?> GetCategoryByNameAsync(string categoryName)
        {
            return await _Context.PhotoCategories.FirstOrDefaultAsync(x => x.CategoryName == categoryName);
        }
    }
}
