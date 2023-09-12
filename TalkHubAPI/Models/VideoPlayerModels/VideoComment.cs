using System;
using System.Collections.Generic;
using TalkHubAPI.Data;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoComment
{
    public int Id { get; set; }

    public string MessageContent { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public int LikeCount { get; set; }

    public int? ReplyId { get; set; }

    public int UserId { get; set; }

    public int VideoId { get; set; }

    public virtual ICollection<VideoComment> InverseReply { get; set; } = new List<VideoComment>();

    public virtual VideoComment? Reply { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;

    public virtual ICollection<VideoCommentsLike> VideoCommentsLikes { get; set; } = new List<VideoCommentsLike>();
}
