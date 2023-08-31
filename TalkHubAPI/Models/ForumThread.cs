using System;
using System.Collections.Generic;
using TalkHubAPI.Models;

namespace TalkHubAPI.Models;

public partial class ForumThread
{
    public int Id { get; set; }

    public string ThreadName { get; set; } = null!;

    public string ThreadDescription { get; set; } = null!;

    public virtual ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();
}
