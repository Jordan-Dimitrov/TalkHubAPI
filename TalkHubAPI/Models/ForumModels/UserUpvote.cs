using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.ForumModels;

public partial class UserUpvote
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MessageId { get; set; }

    public int Rating { get; set; }

    public virtual ForumMessage Message { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
