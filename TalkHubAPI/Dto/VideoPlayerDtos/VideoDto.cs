using TalkHubAPI.Dto.UserDtos;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class VideoDto
    {
        public int Id { get; set; }

        public string VideoName { get; set; } = null!;

        public string Mp4name { get; set; } = null!;

        public string ThumbnailName { get; set; } = null!;

        public string VideoDescription { get; set; } = null!;

        public int LikeCount { get; set; }

        public virtual VideoTagDto Tag { get; set; } = null!;

        public virtual UserDto User { get; set; } = null!;
    }
}
