using TalkHubAPI.Dto.UserDtos;

namespace TalkHubAPI.Dto.VideoPlayerDtos
{
    public class VideoCommentDto
    {
        public int Id { get; set; }

        public string MessageContent { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        public int LikeCount { get; set; }

        public int? ReplyId { get; set; }

        public UserDto User { get; set; }

        public VideoDto Video { get; set; }

    }
}
