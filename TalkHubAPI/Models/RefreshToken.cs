using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
