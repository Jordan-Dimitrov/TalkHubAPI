using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoTag
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Video tag must be between 3 and 30 characters")]
    public string TagName { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
