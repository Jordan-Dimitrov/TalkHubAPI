using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Dtos.UserDtos;

namespace TalkHubAPI.Dtos.PhotosDtos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; } = null!;
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
        public string Description { get; set; } = null!;
        [Required]
        public PhotoCategoryDto Category { get; set; } = null!;
        [Required]
        public UserDto User { get; set; } = null!;
    }
}
