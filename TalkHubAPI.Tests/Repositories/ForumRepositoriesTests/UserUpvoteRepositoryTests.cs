using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Repositories.ForumRepositories;

namespace TalkHubAPI.Tests.Repositories.ForumRepositoriesTests
{
    public class UserUpvoteRepositoryTests
    {
        private readonly Seeder _Seeder;
        public UserUpvoteRepositoryTests()
        {
            _Seeder = new Seeder();
        }
        [Fact]
        public async Task UserUpvoteRepository_GetUserUpvoteAsync_ReturnsUserUpvote()
        {
            int userUpvoteId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserUpvoteRepository userUpvoteRepository = new UserUpvoteRepository(context);

            UserUpvote result = await userUpvoteRepository.GetUserUpvoteAsync(userUpvoteId);

            result.Should().NotBeNull();
            result.Should().BeOfType<UserUpvote>();
        }

    }
}
