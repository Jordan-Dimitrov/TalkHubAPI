using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Data;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;

namespace TalkHubAPI.Tests.Repositories.VideoPlayerRepositoriesTests
{
    public class VideoRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoRepository_GetVideoAsync_ReturnsVideo()
        {
            int videoId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoRepository videoRepository = new VideoRepository(context);

            Video result = await videoRepository.GetVideoAsync(videoId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Video>();
        }
    }
}
