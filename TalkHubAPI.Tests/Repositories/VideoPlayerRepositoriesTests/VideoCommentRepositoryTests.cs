using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Data;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;

namespace TalkHubAPI.Tests.Repositories.VideoPlayerRepositoriesTests
{
    public class VideoCommentRepositoryTests
    {
        private readonly Seeder _Seeder;
        public VideoCommentRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task VideoCommentRepository_GetVideoCommentAsync_ReturnsVideoComment()
        {
            int videoCommentId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            VideoCommentRepository videoCommentRepository = new VideoCommentRepository(context);

            VideoComment result = await videoCommentRepository.GetVideoCommentAsync(videoCommentId);

            result.Should().NotBeNull();
            result.Should().BeOfType<VideoComment>();
        }
    }
}
