using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dto.UserDtos
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters")]
        public string Username { get; set; } = string.Empty;
    }
}
