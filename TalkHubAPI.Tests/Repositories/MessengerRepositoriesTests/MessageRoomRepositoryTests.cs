using FluentAssertions;
using TalkHubAPI.Data;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repositories.MessengerRepositories;

namespace TalkHubAPI.Tests.Repositories.MessengerRepositoriesTests
{
    public class MessageRoomRepositoryTests
    {
        private readonly Seeder _Seeder;
        public MessageRoomRepositoryTests()
        {
            _Seeder = new Seeder();
        }
        [Fact]
        public async Task MessageRoomRepository_GetMessageRoomAsync_ReturnsMessageRoom()
        {
            int messageRoomId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            MessageRoomRepository messageRoomRepository = new MessageRoomRepository(context);

            MessageRoom result = await messageRoomRepository.GetMessageRoomAsync(messageRoomId);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessageRoom>();
        }
    }
}
