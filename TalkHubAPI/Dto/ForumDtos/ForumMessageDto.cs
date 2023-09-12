using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Models;

namespace TalkHubAPI.Dto.ForumDtos
{
    public class ForumMessageDto
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public int? ReplyId { get; set; }

        public UserDto User { get; set; }
        public ForumThreadDto ForumThread { get; set; }

        public DateTime DateCreated { get; set; }

        public int UpvoteCount { get; set; }

        public string MessageContent { get; set; } = null!;
    }
}
