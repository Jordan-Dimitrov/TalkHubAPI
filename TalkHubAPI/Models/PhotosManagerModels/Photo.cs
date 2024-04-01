using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.PhotosManagerModels;

public partial class Photo
{
    public int Id { get; set; }
    [Required]
    public string FileName { get; set; } = null!;
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
    public string Description { get; set; } = null!;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public virtual PhotoCategory Category { get; set; } = null!;
    [Required]
    public virtual User User { get; set; } = null!;
}
