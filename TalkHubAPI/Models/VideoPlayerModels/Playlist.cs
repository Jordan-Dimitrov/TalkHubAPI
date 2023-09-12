using System;
using System.Collections.Generic;
using TalkHubAPI.Data;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class Playlist
{
    public int Id { get; set; }

    public string PlaylistName { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VideoPlaylist> VideoPlaylists { get; set; } = new List<VideoPlaylist>();
}
