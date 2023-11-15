using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Controllers.PhotosManagerControllers;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Controllers.VideoPlayerControllers;
using FluentAssertions;

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
                _UserRepository,_VideoPlaylistRepository,
                _VideoRepository, _MemoryCache);

            IActionResult result = await controller.GetPlaylist(playlistId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
