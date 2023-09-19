using TalkHubAPI.Dto.UserDtos;

namespace TalkHubAPI.Dto.PhotosDtos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = null!;
        public PhotoCategoryDto Category { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
