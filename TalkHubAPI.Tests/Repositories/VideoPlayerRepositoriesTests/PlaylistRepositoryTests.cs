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
    public class PlaylistRepositoryTests
    {
        private readonly Seeder _Seeder;
        public PlaylistRepositoryTests()
        {
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task PlaylistRepository_GetPlaylistAsync_ReturnsPlaylist()
        {
            int playlistId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            PlaylistRepository playlistRepository = new PlaylistRepository(context);

            Playlist result = await playlistRepository.GetPlaylistAsync(playlistId);

            result.Should().NotBeNull();
            result.Should().BeOfType<Playlist>();
        }
    }
}
