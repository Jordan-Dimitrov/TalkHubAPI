using TalkHubAPI.Models;

namespace TalkHubAPI.Helper
{
    public interface IAuthService
    {
        string CreateJwtToken(User user);
        string RefreshToken(User user);
        RefreshToken GenerateRefreshToken();
        void SetRefreshToken(RefreshToken refreshToken);
    }
}
