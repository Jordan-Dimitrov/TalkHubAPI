using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class Video
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
    public DateTime DateCreated { get; set; }
    [Required]
    public int LikeCount { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int TagId { get; set; }

    [Required]
    public virtual VideoTag Tag { get; set; } = null!;

    [Required]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();

    public virtual ICollection<VideoPlaylist> VideoPlaylists { get; set; } = new List<VideoPlaylist>();

    public virtual ICollection<VideoUserLike> VideoUserLikes { get; set; } = new List<VideoUserLike>();
}
