using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalkHubAPI.Models;

namespace TalkHubAPI.Helper
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _Configuration;

        public AuthService(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        public string CreateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _Configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public RefreshToken GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public string RefreshToken(User user)
        {
            throw new NotImplementedException();
        }

        public void SetRefreshToken(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
