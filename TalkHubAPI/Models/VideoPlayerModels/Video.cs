using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class Video
{
    public int Id { get; set; }

    public string VideoName { get; set; } = null!;

    public string Mp4name { get; set; } = null!;

    public string ThumbnailName { get; set; } = null!;

    public string VideoDescription { get; set; } = null!;

    public int LikeCount { get; set; }

    public int UserId { get; set; }

    public int TagId { get; set; }

    public virtual VideoTag Tag { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VideoComment> VideoComments { get; set; } = new List<VideoComment>();

    public virtual ICollection<VideoPlaylist> VideoPlaylists { get; set; } = new List<VideoPlaylist>();

    public virtual ICollection<VideoUserLike> VideoUserLikes { get; set; } = new List<VideoUserLike>();
}
