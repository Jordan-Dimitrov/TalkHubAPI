namespace TalkHubAPI.Dto.MessengerDtos
{
    public class SendMessengerMessageDto
    {
        public string? MessageContent { get; set; }

        public string? FileName { get; set; }
        public int RoomId { get; set; }

    }
}
