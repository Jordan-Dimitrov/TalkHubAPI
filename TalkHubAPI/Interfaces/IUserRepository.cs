using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        Task<bool> UserExistsAsync(int id);
        Task<bool> UsernameExistsAsync(string name);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<bool> SaveAsync();
        Task<bool> UpdateRefreshTokenToUserAsync(User user, RefreshToken refreshToken);
    }
}
