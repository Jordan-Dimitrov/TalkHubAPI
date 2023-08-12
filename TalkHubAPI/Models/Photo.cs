using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public virtual PhotoCategory Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
