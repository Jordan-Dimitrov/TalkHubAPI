using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoCommentsLike
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int VideoCommentId { get; set; }

    public int Rating { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual VideoComment VideoComment { get; set; } = null!;
}
