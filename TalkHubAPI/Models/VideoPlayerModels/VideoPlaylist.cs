using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoPlaylist
{
    public int Id { get; set; }

    [Required]
    public int PlaylistId { get; set; }

    [Required]
    public int VideoId { get; set; }

    [Required]
    public virtual Playlist Playlist { get; set; } = null!;

    [Required]
    public virtual Video Video { get; set; } = null!;
}
