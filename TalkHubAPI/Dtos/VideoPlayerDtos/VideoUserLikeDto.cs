using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.VideoPlayerDtos
{
    public class VideoUserLikeDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int VideoId { get; set; }

        [Required]
        public int Rating { get; set; }

    }
}
