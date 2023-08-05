using Microsoft.EntityFrameworkCore;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TalkHubContext _Context;

        public UserRepository(TalkHubContext context)
        {
            _Context = context;
        }

        public bool CreateUser(User user)
        {
            _Context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _Context.Remove(user);
            return Save();
        }

        public User GetUser(int id)
        {
            return _Context.Users.Find(id);
        }

        public ICollection<User> GetUsers()
        {
            return _Context.Users.ToList();
        }

        public bool Save()
        {
            int saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _Context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _Context.Users.Any(o => o.Id == id);
        }
    }
}
