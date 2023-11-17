using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.VideoPlayerDtos
{
    public class CreatePlaylistDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Playlist name must be between 3 and 30 characters")]
        public string PlaylistName { get; set; } = null!;
    }
}
