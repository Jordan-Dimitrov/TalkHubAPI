using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.MessengerDtos
{
    public class MessageRoomDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Message room must be between 3 and 45 characters")]
        public string RoomName { get; set; } = null!;
    }
}
