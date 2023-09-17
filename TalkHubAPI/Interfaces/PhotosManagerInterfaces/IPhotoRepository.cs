using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Interfaces.PhotosManagerInterfaces
{
    public interface IPhotoRepository
    {
        Task<bool> AddPhotoAsync(Photo photo);
        Task<bool> RemovePhotoAsync(Photo photo);
        Task<bool> UpdatePhotoAsync(Photo photo);
        Task<bool> PhotoExistsAsync(int id);
        Task<bool> SaveAsync();
        Task<Photo> GetPhotoAsync(int id);
        Task<ICollection<Photo>> GetPhotosAsync();
        Task<ICollection<Photo>> GetPhotosByCategoryIdAsync(int categoryId);
        Task<ICollection<Photo>> GetPhotosByUserIdAsync(int userId);
    }
}
