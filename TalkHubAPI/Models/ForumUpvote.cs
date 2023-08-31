using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class ForumUpvote
{
    public int Id { get; set; }

    public int MessageId { get; set; }

    public int UpvoteCount { get; set; }
}
