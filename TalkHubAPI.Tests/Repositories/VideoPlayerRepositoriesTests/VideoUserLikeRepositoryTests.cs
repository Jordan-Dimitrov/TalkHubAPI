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
    public class VideoUserLikeRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoUserLikeRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoUserLikeRepository_GetVideoUserLikeAsync_ReturnsVideoUserLike()
        {
            int videoUserLikeId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoUserLikeRepository videoUserLikeRepository = new VideoUserLikeRepository(context);

            VideoUserLike result = await videoUserLikeRepository.GetVideoUserLikeAsync(videoUserLikeId);

            result.Should().NotBeNull();
            result.Should().BeOfType<VideoUserLike>();
        }
    }
}
