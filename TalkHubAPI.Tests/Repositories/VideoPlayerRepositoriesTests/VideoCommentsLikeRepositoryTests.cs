using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;

namespace TalkHubAPI.Tests.Repositories.VideoPlayerRepositoriesTests
{
    public class VideoCommentsLikeRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoCommentsLikeRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoCommentsLikeRepository_GetVideoCommentsLikeAsync_ReturnsVideoComment()
        {
            int videoCommentsLikeId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoCommentsLikeRepository videoCommentsLikeRepository = new VideoCommentsLikeRepository(context);

            VideoCommentsLike result = await videoCommentsLikeRepository
                .GetVideoCommentsLikeAsync(videoCommentsLikeId);

            result.Should().NotBeNull();
            result.Should().BeOfType<VideoCommentsLike>();
        }
    }
}
