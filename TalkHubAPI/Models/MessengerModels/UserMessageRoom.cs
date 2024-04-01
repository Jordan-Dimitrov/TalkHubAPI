using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.MessengerModels;

public partial class UserMessageRoom
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public virtual MessageRoom Room { get; set; } = null!;

    [Required]
    public virtual User User { get; set; } = null!;
}
