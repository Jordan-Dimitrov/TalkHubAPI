using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoPlaylist
{
    public int Id { get; set; }

    public int PlaylistId { get; set; }

    public int VideoId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
