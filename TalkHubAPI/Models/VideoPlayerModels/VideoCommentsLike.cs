using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoCommentsLike
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int VideoCommentId { get; set; }

    [Required]
    public int Rating { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

    [Required]
    public virtual VideoComment VideoComment { get; set; } = null!;
}
