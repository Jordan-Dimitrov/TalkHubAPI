using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces.ServiceInterfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        RefreshToken GenerateRefreshToken();
        UserPassword CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string GetUsernameFromJwtToken(string token);
        string GetRoleFromJwtToken(string token);
        DateTime GetDateFromJwtToken(string token);
        public void SetRefreshToken(RefreshToken newRefreshToken);
        public void SetJwtToken(string jwtToken);
        public void ClearTokens();
        public string CreateRandomToken();
    }
}
