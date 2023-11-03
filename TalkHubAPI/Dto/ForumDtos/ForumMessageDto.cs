using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Models;

namespace TalkHubAPI.Dto.ForumDtos
{
    public class ForumMessageDto
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public int? ReplyId { get; set; }

        [Required]
        public UserDto User { get; set; }

        [Required]
        public ForumThreadDto ForumThread { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int UpvoteCount { get; set; }

        [Required]
        public string MessageContent { get; set; } = null!;
    }
}
