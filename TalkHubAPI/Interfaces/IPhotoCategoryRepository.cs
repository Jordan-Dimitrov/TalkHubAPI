﻿using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IPhotoCategoryRepository
    {
        bool AddCategory(PhotoCategory category);
        bool RemoveCategory(PhotoCategory category);
        bool UpdateCategory(PhotoCategory category);
        bool CategoriesExist(int id);
        bool Save();
        PhotoCategory GetCategory(int id);
        ICollection<PhotoCategory> GetCategories();
    }
}
