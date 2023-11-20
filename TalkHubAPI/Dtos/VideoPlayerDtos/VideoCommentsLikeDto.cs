using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.VideoPlayerDtos
{
    public class VideoCommentsLikeDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int VideoCommentId { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
