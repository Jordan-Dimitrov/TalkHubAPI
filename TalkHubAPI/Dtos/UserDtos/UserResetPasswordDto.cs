using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.UserDtos
{
    public class UserResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[@#$%^&+=]).{8,}$")]
        public string Password { get; set; } = string.Empty;
    }
}
