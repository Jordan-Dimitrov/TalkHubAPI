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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IVideoUserLikeRepository _VideoUserLikeRepository;
        private readonly IVideoTagRepository _VideoTagRepository;
        public VideoController(IVideoRepository videoRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IFileProcessingService fileProcessingService,
            IVideoUserLikeRepository videoUserLikeRepository,
            IVideoTagRepository videoTagRepository)
        {
            _VideoRepository = videoRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _FileProcessingService = fileProcessingService;
            _VideoUserLikeRepository = videoUserLikeRepository;
            _VideoTagRepository = videoTagRepository;
        }
        [HttpPost("addMedia"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult CreateVideo(IFormFile video, IFormFile thumbnail, [FromForm] CreateVideoDto videoDto)
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

            if (!_VideoTagRepository.VideoTagExists(videoDto.TagId))
            {
                return BadRequest("This tag does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string response1 = _FileProcessingService.UploadMedia(video);

            if (response1 == "Empty" || response1 == "Invalid file format" || response1 == "File already exists")
            {
                return BadRequest(response1);
            }

            string response2 = _FileProcessingService.UploadMedia(thumbnail);

            if (response2 == "Empty" || response2 == "Invalid file format" || response2 == "File already exists")
            {
                return BadRequest(response2);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Video videoToUpload = _Mapper.Map<Video>(videoDto);
            videoToUpload.Tag = _Mapper.Map<VideoTag>(_VideoTagRepository.GetVideoTag(videoToUpload.TagId));

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));

            videoToUpload.ThumbnailName = thumbnail.FileName;
            videoToUpload.Mp4name = video.FileName;
            videoToUpload.LikeCount = 0;
            videoToUpload.User = user;

            if (!_VideoRepository.AddVideo(videoToUpload))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("videosByTag/{tagId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetAllVideosByTag(int tagId)
        {
            if (!_VideoTagRepository.VideoTagExists(tagId))
            {
                return BadRequest("This tag does not exist");
            }
            List<Video> videos = _VideoRepository.GetVideosByTagId(tagId).ToList();
            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(_VideoRepository.GetVideosByTagId(tagId)).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(_UserRepository.GetUser(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(_VideoTagRepository.GetVideoTag(videos[i].TagId));
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

            FileContentResult file = _FileProcessingService.GetMedia(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("thumbnail/{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetThumbnail(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) == "video/mp4")
            {
                return BadRequest(ModelState);
            }

            FileContentResult file = _FileProcessingService.GetMedia(fileName);

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
        public IActionResult HideVideo(int videoId)
        {
            if (!_VideoRepository.VideoExists(videoId))
            {
                return NotFound();
            }

            Video videoToHide = _VideoRepository.GetVideo(videoId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_FileProcessingService.RemoveMedia(videoToHide.Mp4name) ||
                !_FileProcessingService.RemoveMedia(videoToHide.ThumbnailName))
            {
                return BadRequest("Unexpected error");
            }

            videoToHide.Mp4name = "hidden_video.mp4";
            videoToHide.ThumbnailName = "hidden.png";
            videoToHide.VideoDescription = "video was hidden";

            if (!_VideoRepository.UpdateVideo(videoToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating video");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("upvoteVideo/{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult UpvoteVideo([FromQuery] int upvoteValue, int videoId)
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

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!_VideoRepository.VideoExists(videoId))
            {
                return BadRequest("This video does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Video video = _VideoRepository.GetVideo(videoId);

            VideoUserLike userLike = new VideoUserLike();
            userLike.Video = video;
            userLike.User = _UserRepository.GetUserByName(username);



            if (_VideoUserLikeRepository.VideoUserLikeExistsForVideoAndUser(video.Id, video.UserId))
            {
                userLike = _VideoUserLikeRepository.GetVideoUserLikeByVideoAndUser(video.Id, video.UserId);
                if (upvoteValue == userLike.Rating)
                {
                    userLike.Rating = 0;
                    video.LikeCount -= upvoteValue;
                }
                else
                {
                    int temp = userLike.Rating;
                    userLike.Rating = upvoteValue;
                    video.LikeCount += upvoteValue - temp;
                }
            }
            else
            {
                userLike.Rating = upvoteValue;
                video.LikeCount += upvoteValue;
                if (!_VideoUserLikeRepository.AddVideoUserLike(userLike))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_VideoRepository.UpdateVideo(video) || !_VideoUserLikeRepository.UpdateVideoUserLike(userLike))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
