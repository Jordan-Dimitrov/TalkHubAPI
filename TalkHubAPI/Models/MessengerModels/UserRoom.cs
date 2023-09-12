using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.MessengerModels;

public partial class UserRoom
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public virtual MessageRoom Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
