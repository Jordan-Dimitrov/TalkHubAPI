using System;
using System.Collections.Generic;
using TalkHubAPI.Data;

namespace TalkHubAPI.Models;

public partial class MessageRoom
{
    public int Id { get; set; }

    public string RoomName { get; set; } = null!;

    public virtual ICollection<MessengerMessage> MessengerMessages { get; set; } = new List<MessengerMessage>();

    public virtual ICollection<UserMessageRoom> UserMessageRooms { get; set; } = new List<UserMessageRoom>();

    public virtual ICollection<UserRoom> UserRooms { get; set; } = new List<UserRoom>();
}
