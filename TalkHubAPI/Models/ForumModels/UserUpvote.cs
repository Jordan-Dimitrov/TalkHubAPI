using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.ForumModels;

public partial class UserUpvote
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int MessageId { get; set; }

    [Required]
    public int Rating { get; set; }

    [Required]
    public virtual ForumMessage Message { get; set; } = null!;

    [Required]
    public virtual User User { get; set; } = null!;
}
