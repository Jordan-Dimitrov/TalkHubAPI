using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models.PhotosManagerModels;

public partial class PhotoCategory
{
    public int Id { get; set; }
    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 30 characters")]
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
