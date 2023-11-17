using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TalkHubContext _Context;

        public UserRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await _Context.AddAsync(user);
            return await SaveAsync();
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _Context.Remove(user);
            return await SaveAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            int saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _Context.Update(user);
            return await SaveAsync();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _Context.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> UsernameExistsAsync(string name)
        {
            return await _Context.Users.AnyAsync(x => x.Username == name);
        }

        public async Task<User?> GetUserByNameAsync(string username)
        {
            User? user = await _Context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            if (user is not null)
            {
                await _Context.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
            }
            return user;
        }

        public async Task<bool> UpdateRefreshTokenToUserAsync(User user, RefreshToken newRefreshToken)
        {
            if(user.RefreshToken is not null)
            {
                _Context.Remove(user.RefreshToken);
            }

            user.RefreshToken = newRefreshToken;
            return await UpdateUserAsync(user);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            if(refreshToken is null)
            {
                return null;
            }

            User? user = await _Context.Users.Where(x => x.RefreshToken.Token == refreshToken).FirstOrDefaultAsync();

            if (user is not null)
            {
                await _Context.Entry(user).Reference(u => u.RefreshToken).LoadAsync();
            }

            return user;
        }

        public async Task<bool> RefreshTokenExistsForUserAsync(string refreshToken)
        {
            return await _Context.Users.AnyAsync(x => x.RefreshToken.Token == refreshToken);
        }
    }
}
