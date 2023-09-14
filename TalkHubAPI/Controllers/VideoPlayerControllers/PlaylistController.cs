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
        public IActionResult GetPlaylists()
        {
            ICollection<PlaylistDto> playlists = _Mapper.Map<List<PlaylistDto>>(_PlaylistRepository.GetPlaylists());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlists);
        }
        [HttpGet("playlistsByUser/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PlaylistDto>))]
        public IActionResult GetPlaylistsByUser(int userId)
        {
            ICollection<PlaylistDto> playlists = _Mapper.Map<List<PlaylistDto>>(_PlaylistRepository.GetPlaylistsByUserId(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlists);
        }
        [HttpGet("{playlistId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PlaylistDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPlaylist(int playlistId)
        {
            if (!_PlaylistRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            PlaylistDto playlist = _Mapper.Map<PlaylistDto>(_PlaylistRepository.GetPlaylist(playlistId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(playlist);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlaylist([FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_PlaylistRepository.PlaylistExists(playlistCreate.PlaylistName))
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

            if (!_PlaylistRepository.AddPlaylist(playlist))
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
        public IActionResult UpdatePlaylist(int playlistId, [FromBody] PlaylistDto updatedPlaylist)
        {
            if (updatedPlaylist == null || playlistId != updatedPlaylist.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_PlaylistRepository.PlaylistExists(playlistId))
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

            if (!_PlaylistRepository.UpdatePlaylist(playlistToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the playlist");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("addToPlaylist"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddVideoToPlaylist([FromQuery] int videoId, [FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_PlaylistRepository.PlaylistExists(playlistCreate.Id))
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
            Playlist playlist = _Mapper.Map<Playlist>(_PlaylistRepository.GetPlaylist(playlistCreate.Id));
            VideoPlaylist videoPlaylist = new VideoPlaylist();
            videoPlaylist.Playlist = playlist;
            videoPlaylist.Video = _Mapper.Map<Video>(_VideoRepository.GetVideo(videoId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_VideoPlaylistRepository.AddVideoPlaylist(videoPlaylist))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete("deleteFromPlaylist"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteVideoFromPlaylist([FromQuery] int videoId, [FromBody] PlaylistDto playlistCreate)
        {
            if (playlistCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_PlaylistRepository.PlaylistExists(playlistCreate.Id))
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

            if (_VideoPlaylistRepository.VideoPlaylistExistsForVideoAndPlaylist(videoId, playlistCreate.Id))
            {
                return BadRequest("Video in this playlist does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            Playlist playlist = _Mapper.Map<Playlist>(_PlaylistRepository.GetPlaylist(playlistCreate.Id));
            VideoPlaylist videoPlaylist = _Mapper
                .Map<VideoPlaylist>(_VideoPlaylistRepository
                .GetVideoPlaylistByVideoIdAndPlaylistId(videoId, playlist.Id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_VideoPlaylistRepository.RemoveVideoPlaylist(videoPlaylist))
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
        public IActionResult DeletePlaylist(int playlistId)
        {
            if (!_PlaylistRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            Playlist playlistToDelete = _PlaylistRepository.GetPlaylist(playlistId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_PlaylistRepository.RemovePlaylist(playlistToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the playlist");
            }

            return NoContent();
        }
    }
}
