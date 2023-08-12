using TalkHubAPI.Models;

namespace TalkHubAPI.Dto
{
    public class CreatePhotoDto
    {
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
