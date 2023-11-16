using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Repositories.ForumRepositories;

namespace TalkHubAPI.Tests.Repositories.ForumRepositoriesTests
{
    public class ForumThreadRepositoryTests
    {
        private readonly Seeder _Seeder;
        public ForumThreadRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task ForumThreadRepository_GetForumThreadAsync_ReturnsForumThread()
        {
            int forumThreadId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            ForumThreadRepository forumThreadRepository = new ForumThreadRepository(context);

            ForumThread result = await forumThreadRepository.GetForumThreadAsync(forumThreadId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ForumThread>();
        }
    }
}
