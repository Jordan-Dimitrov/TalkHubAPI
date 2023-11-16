using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Data;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repositories.MessengerRepositories;

namespace TalkHubAPI.Tests.Repositories.MessengerRepositoriesTests
{
    public class MessengerMessageRepositoryTests
    {
        private readonly Seeder _Seeder;
        public MessengerMessageRepositoryTests()
        {
            _Seeder = new Seeder();
        }
        [Fact]
        public async Task MessengerMessageRepository_GetMessengerMessageAsync_ReturnsMessengerMessage()
        {
            int messengerMessageId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            MessengerMessageRepository messengerMessageRepository = new MessengerMessageRepository(context);

            MessengerMessage result = await messengerMessageRepository.GetMessengerMessageAsync(messengerMessageId);

            result.Should().NotBeNull();
            result.Should().BeOfType<MessengerMessage>();
        }
    }
}
