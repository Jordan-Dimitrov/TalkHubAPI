using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;

namespace TalkHubAPI.Tests.Repositories.VideoPlayerRepositoriesTests
{
    public class VideoTagRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoTagRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoTagRepository_GetVideoTagAsync_ReturnsVideoTag()
        {
            int videoTagId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoTagRepository videoTagRepository = new VideoTagRepository(context);

            VideoTag result = await videoTagRepository.GetVideoTagAsync(videoTagId);

            result.Should().NotBeNull();
            result.Should().BeOfType<VideoTag>();
        }
    }
}
