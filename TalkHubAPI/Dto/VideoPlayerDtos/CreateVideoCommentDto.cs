namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class CreateVideoCommentDto
    {

        public string MessageContent { get; set; } = null!;

        public int? ReplyId { get; set; }

        public int VideoId { get; set; }
    }
}
