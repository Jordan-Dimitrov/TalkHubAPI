using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.ForumModels;

public partial class ForumMessage
{
    public int Id { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "Message must be between 3 and 1000 characters")]
    public string MessageContent { get; set; } = null!;

    public string? FileName { get; set; }

    public int? ReplyId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public int ForumThreadId { get; set; }

    [Required]
    public int UpvoteCount { get; set; }

    [Required]
    public virtual ForumThread ForumThread { get; set; } = null!;

    public virtual ICollection<ForumMessage> InverseReply { get; set; } = new List<ForumMessage>();

    public virtual ForumMessage? Reply { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserUpvote> UserUpvotes { get; set; } = new List<UserUpvote>();
}
