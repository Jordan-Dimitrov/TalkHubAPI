using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Models;

namespace TalkHubAPI.Dto.ForumDtos
{
    public class CreateForumMessageDto
    {
        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Message must be between 3 and 1000 characters")]
        public string MessageContent { get; set; } = null!;

        public int? ReplyId { get; set; }

        [Required]
        public int ForumThreadId { get; set; }
    }
}
