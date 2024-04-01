using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ConfigurationModels;

namespace TalkHubAPI.Helper
{
    public class AuthService : IAuthService
    {
        private readonly JwtTokenSettings _JwtTokenSettings;
        private readonly RefreshTokenSettings _RefreshTokenSettings;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public AuthService(IOptions<JwtTokenSettings> jwtTokenSettings,
            IHttpContextAccessor httpContextAccessor,
            IOptions<RefreshTokenSettings> refreshTokenSettings)
        {
            _JwtTokenSettings = jwtTokenSettings.Value;
            _HttpContextAccessor = httpContextAccessor;
            _RefreshTokenSettings = refreshTokenSettings.Value;
        }
        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.PermissionType.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_JwtTokenSettings.Token));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_JwtTokenSettings.MinutesExpiry),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TokenExpires = DateTime.Now.AddDays(_RefreshTokenSettings.DaysExpiry),
                TokenCreated = DateTime.Now
            };

            return refreshToken;
        }
        public UserPassword CreatePasswordHash(string password)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                UserPassword pass = new UserPassword();
                pass.PasswordSalt = hmac.Key;
                pass.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return pass;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public string GetUsernameFromJwtToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            string username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return username;
        }
        public string GetRoleFromJwtToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            string role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            return role;
        }
        public DateTime GetDateFromJwtToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            DateTime date = jwtToken.ValidTo;

            return date;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken)
        {
            HttpResponse? response = _HttpContextAccessor.HttpContext?.Response;

            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.TokenExpires,
            };

            response?.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
        public void SetJwtToken(string jwtToken)
        {
            HttpResponse? response = _HttpContextAccessor.HttpContext?.Response;

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = GetDateFromJwtToken(jwtToken),

                HttpOnly = true,
            };

            response?.Cookies.Append("jwtToken", jwtToken, cookieOptions);
        }
        public void ClearTokens()
        {
            HttpResponse? response = _HttpContextAccessor.HttpContext?.Response;

            response?.Cookies.Delete("jwtToken");

            response?.Cookies.Delete("refreshToken");
        }

    }
}
