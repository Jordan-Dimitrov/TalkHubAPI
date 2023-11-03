using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.MessengerModels;

public partial class MessageRoom
{
    public int Id { get; set; }

    [Required]
    [StringLength(45, MinimumLength = 3, ErrorMessage = "Message room must be between 3 and 45 characters")]
    public string RoomName { get; set; } = null!;

    public virtual ICollection<MessengerMessage> MessengerMessages { get; set; } = new List<MessengerMessage>();

    public virtual ICollection<UserMessageRoom> UserMessageRooms { get; set; } = new List<UserMessageRoom>();
}
