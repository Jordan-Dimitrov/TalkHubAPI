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
            _Context.Add(photo);
            return Save();
        }

        public Photo GetPhoto(int id)
        {
            return _Context.Photos.Find(id);
        }

        public ICollection<Photo> GetPhotos()
        {
            return _Context.Photos.ToList();
        }

        public ICollection<Photo> GetPhotosByCategoryId(int categoryId)
        {
            return _Context.Photos.Where(x=>x.CategoryId == categoryId).ToList();
        }
        public ICollection<Photo> GetPhotosByUserId(int userId)
        {
            return _Context.Photos.Where(x => x.UserId == userId).ToList();
        }

        public bool PhotoExists(int id)
        {
            return _Context.Photos.Any(o => o.Id == id);
        }

        public bool RemovePhoto(Photo photo)
        {
            _Context.Remove(photo);
            return Save();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePhoto(Photo photo)
        {
            _Context.Update(photo);
            return Save();
        }
    }
}
