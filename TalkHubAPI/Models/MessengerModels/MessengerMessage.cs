using System;
using System.Collections.Generic;

namespace TalkHubAPI.Models.MessengerModels;

public partial class MessengerMessage
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoomId { get; set; }

    public string? MessageContent { get; set; }

    public string? FileName { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual MessageRoom? Room { get; set; }

    public virtual User? User { get; set; }
}
