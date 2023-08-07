using TalkHubAPI.Data;
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
            throw new NotImplementedException();
        }

        public bool CategoriesExist(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<PhotoCategory> GetCategories()
        {
            throw new NotImplementedException();
        }

        public PhotoCategory GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCategory(PhotoCategory category)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool UpdateCategory(PhotoCategory category)
        {
            throw new NotImplementedException();
        }
    }
}
