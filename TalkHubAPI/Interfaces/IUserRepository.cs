using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUserByName(string username);
        bool UserExists(int id);
        bool UsernameExists(string name);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        bool UpdateRefreshTokenToUser(User user, RefreshToken refreshToken);
    }
}
