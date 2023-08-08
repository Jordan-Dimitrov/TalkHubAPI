using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly TalkHubContext _Context;

        public PhotoRepository(TalkHubContext context)
        {
            _Context = context;
        }
        public bool AddPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Photo GetPhoto(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Photo> GetPhotos()
        {
            throw new NotImplementedException();
        }

        public ICollection<Photo> GetPhotosByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public bool PhotoExists(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
