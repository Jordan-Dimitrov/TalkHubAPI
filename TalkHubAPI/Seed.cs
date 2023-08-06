using Azure.Core;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI
{
    public class Seed
    {
        private readonly TalkHubContext _Context;
        private readonly IAuthService _AuthService;
        public Seed(TalkHubContext context, IAuthService authService)
        {
            _Context = context;
            _AuthService = authService;
        }
        public void SeedTalkHubContext()
        {
            _AuthService.CreatePasswordHash("prototype", out byte[] passwordHash, out byte[] passwordSalt);
            if (!_Context.Users.Any())
            {
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Username = "TOMAAAA",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RefreshToken = _AuthService.GenerateRefreshToken()
                    },
                    new User()
                    {
                        Username = "KristiQn Enchev",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RefreshToken = _AuthService.GenerateRefreshToken()
                    }
                };
                _Context.Users.AddRange(users);
                _Context.SaveChanges();
            }

        }
    }
}
