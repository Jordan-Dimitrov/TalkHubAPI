using TalkHubAPI.Models;

namespace TalkHubAPI.Dto.ForumDtos
{
    public class CreateForumMessageDto
    {
        public string MessageContent { get; set; } = null!;
        public int? ReplyId { get; set; }
        public int ForumThreadId { get; set; }
    }
}
