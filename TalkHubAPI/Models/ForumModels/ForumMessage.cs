﻿using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.ForumModels;

public partial class ForumMessage
{
    public int Id { get; set; }

    public string MessageContent { get; set; } = null!;

    public string? FileName { get; set; }

    public int? ReplyId { get; set; }

    public int UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public int ForumThreadId { get; set; }

    public int UpvoteCount { get; set; }

    public virtual ForumThread ForumThread { get; set; } = null!;

    public virtual ICollection<ForumMessage> InverseReply { get; set; } = new List<ForumMessage>();

    public virtual ForumMessage? Reply { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserUpvote> UserUpvotes { get; set; } = new List<UserUpvote>();
}