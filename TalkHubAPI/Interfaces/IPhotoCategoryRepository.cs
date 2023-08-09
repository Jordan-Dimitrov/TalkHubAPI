using TalkHubAPI.Dto;
using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IPhotoCategoryRepository
    {
        bool AddCategory(PhotoCategory category);
        bool RemoveCategory(PhotoCategory category);
        bool UpdateCategory(PhotoCategory category);
        bool CategoryExists(int id);
        bool Save();
        PhotoCategory GetCategory(int id);
        ICollection<PhotoCategory> GetCategories();
        bool PhotoCategoryExists(string name);
        PhotoCategory GetCategoryByName(string categoryName);
    }
}
