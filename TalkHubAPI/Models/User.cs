using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreated { get; set; }

    public DateTime TokenExpires { get; set; }
}
