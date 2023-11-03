using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.ForumModels;

public partial class ForumThread
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Thread name must be between 3 and 30 characters")]
    public string ThreadName { get; set; } = null!;

    [Required]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Thread name must be between 3 and 255 characters")]
    public string ThreadDescription { get; set; } = null!;

    public virtual ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();
}
