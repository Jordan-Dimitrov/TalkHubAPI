using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Interfaces.PhotosManagerInterfaces
{
    public interface IPhotoRepository
    {
        bool AddPhoto(Photo photo);
        bool RemovePhoto(Photo photo);
        bool UpdatePhoto(Photo photo);
        bool PhotoExists(int id);
        bool Save();
        Photo GetPhoto(int id);
        ICollection<Photo> GetPhotos();
        ICollection<Photo> GetPhotosByCategoryId(int categoryId);
        ICollection<Photo> GetPhotosByUserId(int userId);
    }
}
