using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoUserLike
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int VideoId { get; set; }

    [Required]
    public int Rating { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    [Required]
    public virtual Video Video { get; set; } = null!;
}
