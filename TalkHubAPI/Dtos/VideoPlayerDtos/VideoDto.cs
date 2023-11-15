using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Dtos.UserDtos;

namespace TalkHubAPI.Dtos.VideoPlayerDtos
{
    public class VideoDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Video name must be between 3 and 30 characters")]
        public string VideoName { get; set; } = null!;

        [Required]
        public string Mp4name { get; set; } = null!;

        [Required]
        public string ThumbnailName { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Video description must be between 3 and 255 characters")]
        public string VideoDescription { get; set; } = null!;

        [Required]
        public int LikeCount { get; set; }

        [Required]
        public virtual VideoTagDto Tag { get; set; } = null!;

        [Required]
        public virtual UserDto User { get; set; } = null!;
    }
}
