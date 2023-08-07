using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public int? RefreshTokenId { get; set; }

    public int PermissionType { get; set; }

    public virtual RefreshToken? RefreshToken { get; set; }
}
