using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Dto.PhotosDtos
{
    public class PhotoCategoryDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 30 characters")]
        public string CategoryName { get; set; } = null!;
    }
}
