namespace TalkHubAPI.Dto.MessengerDtos
{
    public class MessengerMessageDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoomId { get; set; }

        public string? MessageContent { get; set; }

        public string? FileName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
