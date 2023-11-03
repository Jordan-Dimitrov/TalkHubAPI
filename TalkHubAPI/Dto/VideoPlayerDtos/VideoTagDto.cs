using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class VideoTagDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Video tag must be between 3 and 30 characters")]
        public string TagName { get; set; } = null!;
    }
}
