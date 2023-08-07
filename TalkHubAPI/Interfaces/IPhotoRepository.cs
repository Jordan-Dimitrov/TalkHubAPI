using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IPhotoRepository
    {
        bool AddPhoto(Photo photo);
        bool RemovePhoto(Photo photo);
        bool UpdatePhoto(Photo photo);
        bool PhotoExists(int id);
        bool Save();
        Photo GetPhoto (int id);
        ICollection<Photo> GetPhotos();
        ICollection<Photo> GetPhotosByCategory(int categoryId);
    }
}
