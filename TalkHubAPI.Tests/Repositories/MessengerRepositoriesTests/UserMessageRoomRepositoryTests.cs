using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repositories.MessengerRepositories;

namespace TalkHubAPI.Tests.Repositories.MessengerRepositoriesTests
{
    public class UserMessageRoomRepositoryTests
    {
        private readonly Seeder _Seeder;
        public UserMessageRoomRepositoryTests()
        {
            _Seeder = new Seeder();
        }
        [Fact]
        public async Task MessengerMessageRepository_GetMessengerMessageAsync_ReturnsMessengerMessage()
        {
            int userMessageRoomId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserMessageRoomRepository userMessageRoomRepository = new UserMessageRoomRepository(context);

            UserMessageRoom result = await userMessageRoomRepository.GetUserMessageRoomAsync(userMessageRoomId);

            result.Should().NotBeNull();
            result.Should().BeOfType<UserMessageRoom>();
        }
    }
}
