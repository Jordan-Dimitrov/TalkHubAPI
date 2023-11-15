using System.ComponentModel.DataAnnotations;
using TalkHubAPI.Models;

namespace TalkHubAPI.Dtos.PhotosDtos
{
    public class CreatePhotoDto
    {
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
        public string Description { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
    }
}
