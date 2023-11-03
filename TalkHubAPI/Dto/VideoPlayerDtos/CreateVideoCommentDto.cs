using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class CreateVideoCommentDto
    {
        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Video comment must be between 3 and 1000 characters")]
        public string MessageContent { get; set; } = null!;

        public int? ReplyId { get; set; }

        [Required]
        public int VideoId { get; set; }
    }
}
