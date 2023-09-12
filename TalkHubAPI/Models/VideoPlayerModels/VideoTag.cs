using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.VideoPlayerModels;

public partial class VideoTag
{
    public int Id { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
