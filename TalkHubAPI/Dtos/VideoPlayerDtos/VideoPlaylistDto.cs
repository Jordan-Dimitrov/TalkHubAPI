using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.VideoPlayerDtos
{
    public class VideoPlaylistDto
    {
        [Required]
        public int PlaylistId { get; set; }

        [Required]
        public int VideoId { get; set; }
    }
}
