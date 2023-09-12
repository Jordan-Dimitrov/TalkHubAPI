using TalkHubAPI.Models;

namespace TalkHubAPI.Dto.PhotosDtos
{
    public class CreatePhotoDto
    {
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
