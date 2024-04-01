using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoComment
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
    public int UserId { get; set; }

    [Required]
    public int VideoId { get; set; }

    public virtual ICollection<VideoComment> InverseReply { get; set; } = new List<VideoComment>();

    public virtual VideoComment? Reply { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    [Required]
    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<VideoCommentsLike> VideoCommentsLikes { get; set; } = new List<VideoCommentsLike>();
}
