using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.ForumDtos
{
    public class UserUpvoteDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
