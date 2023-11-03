using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class CreateVideoDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Video name must be between 3 and 30 characters")]
        public string VideoName { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Video description must be between 3 and 255 characters")]
        public string VideoDescription { get; set; } = null!;

        [Required]
        public int TagId { get; set; }
    }
}
