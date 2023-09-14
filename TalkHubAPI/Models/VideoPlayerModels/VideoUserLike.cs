using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoUserLike
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int VideoId { get; set; }

    public int Rating { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
