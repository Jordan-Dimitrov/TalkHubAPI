namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class CreateVideoDto
    {
        public string VideoName { get; set; } = null!;

        public string VideoDescription { get; set; } = null!;

        public int TagId { get; set; }
    }
}
