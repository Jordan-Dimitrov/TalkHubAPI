using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class PhotoCategory
{
    public int Id { get; set; }

    public string PhotoName { get; set; } = null!;

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
