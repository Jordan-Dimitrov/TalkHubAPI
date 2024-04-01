using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Repositories.ForumRepositories;

namespace TalkHubAPI.Tests.Repositories.ForumRepositoriesTests
{
    public class ForumMessageRepositoryTests
    {
        private readonly Seeder _Seeder;
        public ForumMessageRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task ForumMessageRepository_GetForumMessageAsync_ReturnsForumMessage()
        {
            int forumMessageId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            ForumMessageRepository forumMessageRepository = new ForumMessageRepository(context);

            ForumMessage result = await forumMessageRepository.GetForumMessageAsync(forumMessageId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ForumMessage>();
        }
    }
}
