using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Dto.VideoPlayerDtos;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repository.VideoPlayerRepositories;
using Azure;
using Microsoft.Extensions.Caching.Memory;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        private readonly IVideoRepository _VideoRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly string _VideosCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IVideoUserLikeRepository _VideoUserLikeRepository;
        private readonly IVideoTagRepository _VideoTagRepository;
        private IPlaylistRepository _PlaylistRepository;
        public VideoController(IVideoRepository videoRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IFileProcessingService fileProcessingService,
            IVideoUserLikeRepository videoUserLikeRepository,
            IVideoTagRepository videoTagRepository,
            IPlaylistRepository playlistRepository,
            IMemoryCache memoryCache)
        {
            _VideoRepository = videoRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _FileProcessingService = fileProcessingService;
            _VideoUserLikeRepository = videoUserLikeRepository;
            _VideoTagRepository = videoTagRepository;
            _PlaylistRepository = playlistRepository;
            _MemoryCache = memoryCache;
            _VideosCacheKey = "videos";
        }

        [HttpGet("{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(VideoDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVideo(int videoId)
        {
            if (!await _VideoRepository.VideoExistsAsync(videoId))
            {
                return NotFound();
            }
            Video video = await _VideoRepository.GetVideoAsync(videoId);
            VideoDto videoDto = _Mapper.Map<VideoDto>(await _VideoRepository.GetVideoAsync(videoId));
            videoDto.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(video.UserId));
            videoDto.Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(video.TagId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(videoDto);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [RequestSizeLimit(100_000_000)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateVideo(IFormFile video, IFormFile thumbnail, [FromForm] CreateVideoDto videoDto)
        {
            if (video == null || video.Length == 0 || thumbnail == null || thumbnail.Length == 0 || videoDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_FileProcessingService.GetContentType(video.FileName) != "video/mp4" || 
                _FileProcessingService.GetContentType(thumbnail.FileName) == "video/mp4")
            {
                return BadRequest(ModelState);
            }

            if (await _VideoRepository.VideoExistsAsync(videoDto.VideoName))
            {
                ModelState.AddModelError("", "Video with such name already exists");
                return StatusCode(422, ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _VideoTagRepository.VideoTagExistsAsync(videoDto.TagId))
            {
                return BadRequest("This tag does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string response2 = await _FileProcessingService.UploadImageAsync(thumbnail);

            if (response2 == "Empty" || response2 == "Invalid file format" || response2 == "File already exists")
            {
                return BadRequest(response2);
            }

            string response1 = await _FileProcessingService.UploadVideoAsync(video);

            if (response1 == "Empty" || response1 == "Invalid file format" || response1 == "File already exists")
            {
                return BadRequest(response1);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string cacheKey = _VideosCacheKey + $"_{videoDto.TagId}";

            Video videoToUpload = _Mapper.Map<Video>(videoDto);
            videoToUpload.Tag = _Mapper.Map<VideoTag>(await _VideoTagRepository.GetVideoTagAsync(videoToUpload.TagId));

            User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(username));

            videoToUpload.ThumbnailName = response2;
            videoToUpload.Mp4name = response1;
            videoToUpload.LikeCount = 0;
            videoToUpload.User = user;

            if (!await _VideoRepository.AddVideoAsync(videoToUpload))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return Ok("Successfully created");
        }

        [HttpGet("tag/{tagId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByTag(int tagId)
        {
            if (!await _VideoTagRepository.VideoTagExistsAsync(tagId))
            {
                return BadRequest("This tag does not exist");
            }

            string cacheKey = _VideosCacheKey + $"_{tagId}";

            List<VideoDto> videoDtos = _MemoryCache.Get<List<VideoDto>>(cacheKey);

            if (videoDtos == null)
            {
                videoDtos = _Mapper.Map<List<VideoDto>>(await _VideoRepository.GetVideosByTagIdAsync(tagId)).ToList();
                List<Video> videos = (await _VideoRepository.GetVideosByTagIdAsync(tagId)).ToList();

                for (int i = 0; i < videos.Count; i++)
                {
                    videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                    videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(videos[i].TagId));
                }

                _MemoryCache.Set(cacheKey, videoDtos, TimeSpan.FromMinutes(1));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(videoDtos);
        }

        [HttpGet("user/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByUser(int userId)
        {
            if (!await _UserRepository.UserExistsAsync(userId))
            {
                return BadRequest("This user does not exist");
            }

            List<Video> videos = (await _VideoRepository.GetVideosByUserIdAsync(userId)).ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(await _VideoRepository
                .GetVideosByUserIdAsync(userId)).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(videos[i].TagId));
            }

            return Ok(videoDtos);
        }

        [HttpGet("playlist/{playlistId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByPlaylistId(int playlistId)
        {
            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return BadRequest("This playlist does not exist");
            }

            List<Video> videos = (await _VideoRepository.GetVideosByPlaylistIdAsync(playlistId)).ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(await _VideoRepository
                .GetVideosByPlaylistIdAsync(playlistId))
                .ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(videos[i].TagId));
            }

            return Ok(videoDtos);
        }

        [HttpGet("video/{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetVideo(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) != "video/mp4")
            {
                return BadRequest(ModelState);
            }

            FileStreamResult file = _FileProcessingService.GetVideo(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("thumbnail/{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetThumbnail(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) != "image/webp")
            {
                return BadRequest(ModelState);
            }

            FileContentResult file = await _FileProcessingService.GetImageAsync(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpPut("hide/{videoId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideVideo(int videoId)
        {
            if (!await _VideoRepository.VideoExistsAsync(videoId))
            {
                return NotFound();
            }

            Video videoToHide = await _VideoRepository.GetVideoAsync(videoId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _FileProcessingService.RemoveMediaAsync(videoToHide.Mp4name) ||
                !await _FileProcessingService.RemoveMediaAsync(videoToHide.ThumbnailName))
            {
                return BadRequest("Unexpected error");
            }

            string cacheKey = _VideosCacheKey + $"_{videoToHide.TagId}";

            videoToHide.Mp4name = "hidden_video.mp4";
            videoToHide.ThumbnailName = "hidden.png";
            videoToHide.VideoDescription = "video was hidden";

            if (!await _VideoRepository.UpdateVideoAsync(videoToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating video");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }

        [HttpPut("upvote/{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> UpvoteVideo([FromQuery] int upvoteValue, int videoId)
        {
            if (upvoteValue != 1 && upvoteValue != -1)
            {
                return BadRequest(ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _VideoRepository.VideoExistsAsync(videoId))
            {
                return BadRequest("This video does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Video video = await _VideoRepository.GetVideoAsync(videoId);
            User user = await _UserRepository.GetUserByNameAsync(username);

            string cacheKey = _VideosCacheKey + $"_{video.TagId}";

            if (!await _VideoUserLikeRepository.VideoUserLikeExistsForVideoAndUserAsync(video.Id, user.Id))
            {
                VideoUserLike videoUserLikeToAdd = new VideoUserLike();
                videoUserLikeToAdd.Rating = upvoteValue;
                videoUserLikeToAdd.Video = video;
                videoUserLikeToAdd.User = user;
                video.LikeCount += upvoteValue;

                if (!await _VideoUserLikeRepository.AddVideoUserLikeAsync(videoUserLikeToAdd))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }

            VideoUserLike videoUserLike = await _VideoUserLikeRepository
                .GetVideoUserLikeByVideoAndUserAsync(video.Id, user.Id);

            if (upvoteValue == videoUserLike.Rating)
            {
                videoUserLike.Rating = 0;
                video.LikeCount -= upvoteValue;
            }
            else
            {
                int temp = videoUserLike.Rating;
                videoUserLike.Rating = upvoteValue;
                video.LikeCount += upvoteValue - temp;
            }

            if (!await _VideoRepository.UpdateVideoAsync(video) 
                || !await _VideoUserLikeRepository.UpdateVideoUserLikeAsync(videoUserLike))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }
    }
}
