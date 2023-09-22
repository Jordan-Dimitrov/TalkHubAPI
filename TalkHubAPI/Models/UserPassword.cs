namespace TalkHubAPI.Models
{
    public class UserPassword
    {
        public byte[] PasswordHash { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;
    }
}
