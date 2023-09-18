using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.VideoPlayerDtos;
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
        private readonly IVideoPlaylistRepository _VideoPlaylistRepository;
        private readonly IVideoRepository _VideoRepository;
        public PlaylistController(IPlaylistRepository playlistRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IVideoPlaylistRepository videoPlaylistRepository,
            IVideoRepository videoRepository)
        {
            _PlaylistRepository = playlistRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _VideoPlaylistRepository = videoPlaylistRepository;
            _VideoRepository = videoRepository;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlaylistDto>))]
        public async Task<IActionResult> GetPlaylists()
        {
            ICollection<PlaylistDto> playlists = _Mapper.Map<List<PlaylistDto>>(await _PlaylistRepository
                .GetPlaylistsAsync());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlists);
        }
        [HttpGet("playlistsByUser/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlaylistDto>))]
        public async Task<IActionResult> GetPlaylistsByUser(int userId)
        {
            ICollection<PlaylistDto> playlists = _Mapper.Map<List<PlaylistDto>>(await _PlaylistRepository
                .GetPlaylistsByUserIdAsync(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlists);
        }
        [HttpGet("{playlistId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PlaylistDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPlaylist(int playlistId)
        {
            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return NotFound();
            }

            PlaylistDto playlist = _Mapper.Map<PlaylistDto>(await _PlaylistRepository
                .GetPlaylistAsync(playlistId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlist);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (await _PlaylistRepository.PlaylistExistsAsync(playlistCreate.PlaylistName))
            {
                ModelState.AddModelError("", "Playlist already exists");
                return StatusCode(422, ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            Playlist playlist = _Mapper.Map<Playlist>(playlistCreate);
            playlist.User = user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.AddPlaylistAsync(playlist))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{playlistId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePlaylist(int playlistId, [FromBody] PlaylistDto updatedPlaylist)
        {
            if (updatedPlaylist == null || playlistId != updatedPlaylist.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return NotFound();
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            Playlist playlistToUpdate = _Mapper.Map<Playlist>(updatedPlaylist);
            playlistToUpdate.User = user;

            if (!await _PlaylistRepository.UpdatePlaylistAsync(playlistToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the playlist");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("addToPlaylist"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddVideoToPlaylist([FromQuery] int videoId, [FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistCreate.Id))
            {
                return BadRequest("This playlist does not exist");
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            Playlist playlist = _Mapper.Map<Playlist>(await _PlaylistRepository.GetPlaylistAsync(playlistCreate.Id));
            VideoPlaylist videoPlaylist = new VideoPlaylist();
            videoPlaylist.Playlist = playlist;
            videoPlaylist.Video = _Mapper.Map<Video>(await _VideoRepository.GetVideoAsync(videoId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _VideoPlaylistRepository.AddVideoPlaylistAsync(videoPlaylist))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete("deleteFromPlaylist"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteVideoFromPlaylist([FromQuery] int videoId, [FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistCreate.Id))
            {
                return BadRequest("This playlist does not exist");
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _VideoPlaylistRepository.VideoPlaylistExistsForVideoAndPlaylistAsync(videoId, playlistCreate.Id))
            {
                return BadRequest("Video in this playlist does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            Playlist playlist = _Mapper.Map<Playlist>(await _PlaylistRepository.GetPlaylistAsync(playlistCreate.Id));

            VideoPlaylist videoPlaylist = _Mapper
                .Map<VideoPlaylist>(await _VideoPlaylistRepository
                .GetVideoPlaylistByVideoIdAndPlaylistIdAsync(videoId, playlist.Id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _VideoPlaylistRepository.RemoveVideoPlaylistAsync(videoPlaylist))
            {
                ModelState.AddModelError("", "Something went wrong deleting the playlist");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{playlistId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return NotFound();
            }

            Playlist playlistToDelete = await _PlaylistRepository.GetPlaylistAsync(playlistId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _PlaylistRepository.RemovePlaylistAsync(playlistToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the playlist");
            }

            return NoContent();
        }
    }
}
