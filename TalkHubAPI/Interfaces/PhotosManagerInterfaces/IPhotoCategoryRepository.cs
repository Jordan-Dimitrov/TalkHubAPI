using TalkHubAPI.Dto;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Interfaces.PhotosManagerInterfaces
{
    public interface IPhotoCategoryRepository
    {
        Task<bool> AddCategoryAsync(PhotoCategory category);
        Task<bool> RemoveCategoryAsync(PhotoCategory category);
        Task<bool> UpdateCategoryAsync(PhotoCategory category);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<PhotoCategory?> GetCategoryAsync(int id);
        Task<ICollection<PhotoCategory>> GetCategoriesAsync();
        Task<bool> PhotoCategoryExistsAsync(string name);
        Task<PhotoCategory?> GetCategoryByNameAsync(string categoryName);
    }
}
