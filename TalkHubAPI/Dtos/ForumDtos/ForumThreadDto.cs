using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dtos.ForumDtos
{
    public class ForumThreadDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Thread name must be between 3 and 30 characters")]
        public string ThreadName { get; set; } = null!;

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Thread name must be between 3 and 255 characters")]
        public string ThreadDescription { get; set; } = null!;
    }
}
