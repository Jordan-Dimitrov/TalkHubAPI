using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class Playlist
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Playlist name must be between 3 and 30 characters")]
    public string PlaylistName { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<VideoPlaylist> VideoPlaylists { get; set; } = new List<VideoPlaylist>();
}
