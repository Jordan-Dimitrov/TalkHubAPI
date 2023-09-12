﻿using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class VideoCommentsLike
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int VideoCommentId { get; set; }

    public int Rating { get; set; }

    public virtual VideoComment VideoComment { get; set; } = null!;
}
