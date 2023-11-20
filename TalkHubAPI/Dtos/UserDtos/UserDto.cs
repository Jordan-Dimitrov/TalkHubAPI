using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.UserDtos
{
    public class UserDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int SubscriberCount { get; set; }
    }
}
