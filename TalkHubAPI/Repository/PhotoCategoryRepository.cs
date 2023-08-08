﻿using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Repository
{
    public class PhotoCategoryRepository : IPhotoCategoryRepository
    {
        private readonly TalkHubContext _Context;

        public PhotoCategoryRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddCategory(PhotoCategory category)
        {
            _Context.Add(category);
            return Save();
        }

        public bool CategoryExists(int id)
        {
            return _Context.PhotoCategories.Any(o => o.Id == id);
        }

        public ICollection<PhotoCategory> GetCategories()
        {
            return _Context.PhotoCategories.ToList();
        }

        public PhotoCategory GetCategory(int id)
        {
            return _Context.PhotoCategories.Find(id);
        }
        public bool RemoveCategory(PhotoCategory category)
        {
            _Context.Remove(category);
            return Save();
        }
        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UpdateCategory(PhotoCategory category)
        {
            _Context.Update(category);
            return Save();
        }

    }
}
