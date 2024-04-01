using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;

namespace TalkHubAPI.Tests.Repositories.VideoPlayerRepositoriesTests
{
    public class VideoPlaylistRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoPlaylistRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoPlaylistRepository_GetVideoPlaylistAsync_ReturnsVideoPlaylist()
        {
            int videoPlaylistId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoPlaylistRepository videoPlaylistRepository = new VideoPlaylistRepository(context);

            VideoPlaylist result = await videoPlaylistRepository.GetVideoPlaylistAsync(videoPlaylistId);

            result.Should().NotBeNull();
            result.Should().BeOfType<VideoPlaylist>();
        }
    }
}
