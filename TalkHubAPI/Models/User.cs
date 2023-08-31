using System;
using System.Collections.Generic;
using TalkHubAPI.Models;

namespace TalkHubAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public int? RefreshTokenId { get; set; }

    public int PermissionType { get; set; }

    public virtual ICollection<ForumMessage> ForumMessages { get; set; } = new List<ForumMessage>();

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

    public virtual RefreshToken? RefreshToken { get; set; }
}
