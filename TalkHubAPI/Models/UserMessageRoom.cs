using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models;

public partial class UserMessageRoom
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public virtual MessageRoom Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
