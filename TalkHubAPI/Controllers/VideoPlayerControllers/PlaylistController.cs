using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Threading;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistRepository _PlaylistRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly string _PlaylistsCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IVideoPlaylistRepository _VideoPlaylistRepository;
        private readonly IVideoRepository _VideoRepository;
        public PlaylistController(IPlaylistRepository playlistRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IVideoPlaylistRepository videoPlaylistRepository,
            IVideoRepository videoRepository,
            IMemoryCache memoryCache)
        {
            _PlaylistRepository = playlistRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _VideoPlaylistRepository = videoPlaylistRepository;
            _VideoRepository = videoRepository;
            _MemoryCache = memoryCache;
            _PlaylistsCacheKey = "playlists";
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlaylistDto>))]
        public async Task<IActionResult> GetPlaylists()
        {
            ICollection<PlaylistDto>? playlists = _MemoryCache.Get<List<PlaylistDto>>(_PlaylistsCacheKey);

            if (playlists is null)
            {
                playlists = _Mapper.Map<List<PlaylistDto>>(await _PlaylistRepository.GetPlaylistsAsync());

                _MemoryCache.Set(_PlaylistsCacheKey, playlists, TimeSpan.FromMinutes(1));
            }

            return Ok(playlists);
        }

        [HttpGet("playlists/user/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlaylistDto>))]
        public async Task<IActionResult> GetPlaylistsByUser(int userId)
        {
            if (!await _UserRepository.UserExistsAsync(userId))
            {
                return BadRequest("This user does not exist");
            }

            if(!await _PlaylistRepository.PlaylistExistsForUserAsync(userId))
            {
                return BadRequest("Playlist does not exist for user");
            }

            ICollection<PlaylistDto> playlists = _Mapper.Map<List<PlaylistDto>>(await _PlaylistRepository
                .GetPlaylistsByUserIdAsync(userId));

            return Ok(playlists);
        }

        [HttpGet("{playlistId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PlaylistDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPlaylist(int playlistId)
        {
            PlaylistDto playlist = _Mapper.Map<PlaylistDto>(await _PlaylistRepository
                .GetPlaylistAsync(playlistId));

            if (playlist is null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate is null)
            {
                return BadRequest(ModelState);
            }

            if (await _PlaylistRepository.PlaylistExistsAsync(playlistCreate.PlaylistName))
            {
                ModelState.AddModelError("", "Playlist with such name already exists");
                return StatusCode(422, ModelState);
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Playlist playlist = _Mapper.Map<Playlist>(playlistCreate);
            playlist.User = user;

            if (!await _PlaylistRepository.AddPlaylistAsync(playlist))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_PlaylistsCacheKey);

            return Ok("Successfully created");
        }

        [HttpPut("{playlistId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePlaylist(int playlistId, [FromBody] PlaylistDto updatedPlaylist)
        {
            if (updatedPlaylist is null || playlistId != updatedPlaylist.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return NotFound();
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Playlist playlistToUpdate = _Mapper.Map<Playlist>(updatedPlaylist);
            playlistToUpdate.User = user;

            if (!await _PlaylistRepository.UpdatePlaylistAsync(playlistToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the playlist");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_PlaylistsCacheKey);

            return NoContent();
        }

        [HttpPost("video"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddVideoToPlaylist([FromBody] VideoPlaylistDto videoPlaylistDto)
        {

            if (videoPlaylistDto is null)
            {
                return BadRequest(ModelState);
            }

            Playlist? playlist = await _PlaylistRepository.GetPlaylistAsync(videoPlaylistDto.PlaylistId);

            if (playlist is null)
            {
                return BadRequest("This playlist does not exist");
            }

            Video? video = await _VideoRepository.GetVideoAsync(videoPlaylistDto.VideoId);

            if (video is null)
            {
                return BadRequest("This video does not exist");
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if(playlist.User != user)
            {
                return BadRequest("Playlist does not belong to user");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoPlaylist videoPlaylist = _Mapper.Map<VideoPlaylist>(videoPlaylistDto);
            videoPlaylist.Playlist = playlist;
            videoPlaylist.Video = video;

            if (!await _VideoPlaylistRepository.AddVideoPlaylistAsync(videoPlaylist))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_PlaylistsCacheKey);

            return Ok("Successfully created");
        }

        [HttpDelete("video"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteVideoFromPlaylist([FromBody] VideoPlaylistDto videoPlaylistDto)
        {
            if (videoPlaylistDto is null)
            {
                return BadRequest(ModelState);
            }

            Playlist? playlist = await _PlaylistRepository.GetPlaylistAsync(videoPlaylistDto.PlaylistId);

            if (playlist is null)
            {
                return BadRequest("This playlist does not exist");
            }

            Video? video = await _VideoRepository.GetVideoAsync(videoPlaylistDto.VideoId);

            if (video is null)
            {
                return BadRequest("This video does not exist");
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (playlist.User != user)
            {
                return BadRequest("Playlist does not belong to user");
            }

            VideoPlaylist videoPlaylist = _Mapper
                .Map<VideoPlaylist>(await _VideoPlaylistRepository
                .GetVideoPlaylistByVideoIdAndPlaylistIdAsync(videoPlaylistDto.VideoId, playlist.Id));

            if (videoPlaylist is null)
            {
                return BadRequest("Video in this playlist does not exist!");
            }

            if (!await _VideoPlaylistRepository.RemoveVideoPlaylistAsync(videoPlaylist))
            {
                ModelState.AddModelError("", "Something went wrong deleting the playlist");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_PlaylistsCacheKey);

            return NoContent();
        }

        [HttpDelete("{playlistId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            Playlist? playlistToDelete = await _PlaylistRepository.GetPlaylistAsync(playlistId);

            if (playlistToDelete is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.RemovePlaylistAsync(playlistToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the playlist");
            }

            _MemoryCache.Remove(_PlaylistsCacheKey);

            return NoContent();
        }
    }
}
