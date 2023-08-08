using TalkHubAPI.Models;

namespace TalkHubAPI.Dto
{
    public class CreatePhotoDto
    {
        public string FileName { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = null!;
        public PhotoCategoryDto Category { get; set; } = null!;
    }
}
