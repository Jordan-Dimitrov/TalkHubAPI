using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Controllers.VideoPlayerControllers;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Tests.Controller.VideoPlayerControllersTests
{
    public class PlaylistControllerTests
    {
        private readonly IPlaylistRepository _PlaylistRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly IMemoryCache _MemoryCache;
        private readonly IVideoPlaylistRepository _VideoPlaylistRepository;
        private readonly IVideoRepository _VideoRepository;
        public PlaylistControllerTests()
        {
            _PlaylistRepository = A.Fake<IPlaylistRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _UserRepository = A.Fake<IUserRepository>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _VideoPlaylistRepository = A.Fake<IVideoPlaylistRepository>();
            _VideoRepository = A.Fake<IVideoRepository>();
        }

        [Fact]
        public async Task PlaylistController_GetPlaylist_ReturnOk()
        {
            int playlistId = 1;
            PlaylistDto playlistDto = A.Fake<PlaylistDto>();
            Playlist playlist = A.Fake<Playlist>();

            A.CallTo(() => _PlaylistRepository.GetPlaylistAsync(playlistId)).Returns(playlist);
            PlaylistController controller = new PlaylistController(_PlaylistRepository,
                _Mapper, _AuthService,
                _UserRepository, _VideoPlaylistRepository,
                _VideoRepository);

            IActionResult result = await controller.GetPlaylist(playlistId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
