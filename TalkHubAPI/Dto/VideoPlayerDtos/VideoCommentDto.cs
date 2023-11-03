using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Dto.UserDtos;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class VideoCommentDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Video comment must be between 3 and 1000 characters")]
        public string MessageContent { get; set; } = null!;

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int LikeCount { get; set; }

        public int? ReplyId { get; set; }

        [Required]
        public UserDto User { get; set; }

        [Required]
        public VideoDto Video { get; set; }

    }
}
