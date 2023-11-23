using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dtos.UserDtos;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Models.VideoPlayerModels;
using Azure;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Interfaces.ServiceInterfaces;

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
        private readonly IPlaylistRepository _PlaylistRepository;
        private readonly IBackgroundQueue _BackgroundQueue;
        private readonly IUserSubscribtionRepository _UserSubscribtionRepository;
        public VideoController(IVideoRepository videoRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IFileProcessingService fileProcessingService,
            IVideoUserLikeRepository videoUserLikeRepository,
            IVideoTagRepository videoTagRepository,
            IPlaylistRepository playlistRepository,
            IBackgroundQueue backgroundQueue,
            IUserSubscribtionRepository userSubscribtionRepository)
        {
            _VideoRepository = videoRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _FileProcessingService = fileProcessingService;
            _VideoUserLikeRepository = videoUserLikeRepository;
            _VideoTagRepository = videoTagRepository;
            _PlaylistRepository = playlistRepository;
            _BackgroundQueue = backgroundQueue;
            _UserSubscribtionRepository = userSubscribtionRepository;
        }

        [HttpGet("{videoId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(VideoDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVideo(int videoId)
        {
            Video? video = await _VideoRepository.GetVideoAsync(videoId);

            if (video is null)
            {
                return NotFound();
            }

            VideoDto videoDto = _Mapper.Map<VideoDto>(video);
            videoDto.User = _Mapper.Map<UserDto>(video.User);
            videoDto.Tag = _Mapper.Map<VideoTagDto>(video.Tag);

            return Ok(videoDto);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201, Type = typeof(VideoUploadResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateVideo(IFormFile video, IFormFile thumbnail, [FromForm] CreateVideoDto videoDto)
        {
            if (videoDto is null 
                || !_FileProcessingService.VideoMimeTypeValid(video) 
                || !_FileProcessingService.ImageMimeTypeValid(thumbnail))
            {
                return BadRequest(ModelState);
            }

            if (await _VideoRepository.VideoExistsAsync(videoDto.VideoName))
            {
                ModelState.AddModelError("", "Video with such name already exists");
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

            VideoTag? tag = await _VideoTagRepository.GetVideoTagAsync(videoDto.TagId);

            if (tag is null)
            {
                return BadRequest("This tag does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string response2 = await _FileProcessingService.UploadImageAsync(thumbnail);

            if (response2 == "File already exists")
            {
                return BadRequest(response2);
            }

            VideoUploadResponse response1 = await _FileProcessingService.UploadVideoAsync(video);

            if (response1.Error == "File already exists")
            {
                return BadRequest(response1);
            }

            Video videoToUpload = _Mapper.Map<Video>(videoDto);
            videoToUpload.Tag = _Mapper.Map<VideoTag>(tag);

            videoToUpload.ThumbnailName = response2;
            videoToUpload.Mp4name = response1.WebmFileName;
            videoToUpload.LikeCount = 0;
            videoToUpload.User = user;
            videoToUpload.DateCreated = DateTime.Now;

            if (!await _VideoRepository.AddVideoAsync(videoToUpload))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(response1);
        }

        [HttpGet("taskId/{taskId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Expire3")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetConversionStatus(Guid taskId)
        {
            return Ok(_BackgroundQueue.GetStatus(taskId));
        }

        [HttpGet("tag/{tagId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByTag(int tagId)
        {
            VideoTag? tag = await _VideoTagRepository.GetVideoTagAsync(tagId);

            if (tag is null)
            {
                return BadRequest("This tag does not exist");
            }

            List<Video> videos = (await _VideoRepository.GetVideosByTagIdAsync(tagId)).ToList();
            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(videos);

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(tag);
            }

            return Ok(videoDtos);
        }

        [HttpGet("subscribtions"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetVideosBySubscribtions()
        {
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

            List<Video> videos = (await _VideoRepository.GetAllUserSubscribedChannelVideosAsync(user.Id))
                .ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(videos);

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(videos[i].TagId));
            }

            return Ok(videoDtos);
        }

        [HttpGet("user/{userId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByUser(int userId)
        {
            User? user = await _UserRepository.GetUserAsync(userId);

            if (user is null)
            {
                return BadRequest("This user does not exist");
            }

            List<Video> videos = (await _VideoRepository.GetVideosByUserIdAsync(userId)).ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(videos);

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(user);
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository
                    .GetVideoTagAsync(videos[i].TagId));
            }

            return Ok(videoDtos);
        }

        [HttpGet("recommended"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetRecommendedVideosByUser()
        {
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

            List<Video> videos = (await _VideoRepository.GetRecommendedVideosByUserIdAsync(user.Id)).ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(videos);

            for (int i = 0; i < videos.Count; i++)
            {
                videoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(videos[i].UserId));
                videoDtos[i].Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(videos[i].TagId));
            }

            return Ok(videoDtos);
        }

        [HttpGet("playlist/{playlistId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllVideosByPlaylistId(int playlistId)
        {
            if (!await _PlaylistRepository.PlaylistExistsAsync(playlistId))
            {
                return BadRequest("This playlist does not exist");
            }

            List<Video> videos = (await _VideoRepository.GetVideosByPlaylistIdAsync(playlistId)).ToList();

            List<VideoDto> videoDtos = _Mapper.Map<List<VideoDto>>(videos);

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
            if (_FileProcessingService.GetContentType(fileName) != "video/webm")
            {
                return BadRequest(ModelState);
            }

            FileStreamResult file = _FileProcessingService.GetVideo(fileName);

            if (file is null)
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

            if (file is null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpPut("hide/{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideVideo(int videoId)
        {
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

            Video? videoToHide = await _VideoRepository.GetVideoAsync(videoId);

            if (videoToHide is null)
            {
                return NotFound();
            }

            if(videoToHide.User != user && user.PermissionType != UserRole.Admin)
            {
                return BadRequest("Video does not belong to user");
            }

            if (!await _FileProcessingService.RemoveMediaAsync(videoToHide.Mp4name) ||
                !await _FileProcessingService.RemoveMediaAsync(videoToHide.ThumbnailName))
            {
                return BadRequest("Unexpected error");
            }

            videoToHide.Mp4name = "hidden_video.webm";
            videoToHide.ThumbnailName = "hidden.webp";
            videoToHide.VideoDescription = "video was hidden";

            if (!await _VideoRepository.UpdateVideoAsync(videoToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating video");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("user-subscribtion/{userId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Expire3")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetUserSubcsribtion(int userId)
        {
            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? subscriber = await _UserRepository.GetUserByNameAsync(username);

            if (subscriber is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            User? channel = await _UserRepository.GetUserAsync(userId);

            if (channel is null)
            {
                return BadRequest("This channel does not exist");
            }

            UserSubscribtionDto? userSubscribtion = _Mapper
                .Map<UserSubscribtionDto>(await _UserSubscribtionRepository
                .GetUserSubscribtionByChannelAndSubscriberAsync(channel.Id, subscriber.Id));

            if (userSubscribtion is null)
            {
                return NotFound();
            }

            return Ok(userSubscribtion);
        }

        [HttpPut("subscribe-user-channel/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SubscribeToUserChannel(int userId)
        {
            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? subscriber = await _UserRepository.GetUserByNameAsync(username);

            if (subscriber is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            User? channel = await _UserRepository.GetUserAsync(userId);

            if (channel is null)
            {
                return BadRequest("This channel does not exist");
            }

            if (await _UserSubscribtionRepository
                .UserSubscribtionExistsForChannelAndSubscriberAsync(channel.Id, subscriber.Id))
            {
                return BadRequest("User is already subscribed");
            }

            channel.SubscriberCount += 1;
            UserSubscribtion userSubscribtionToAdd = new UserSubscribtion();
            userSubscribtionToAdd.UserSubscriber = subscriber;
            userSubscribtionToAdd.UserChannel = channel;

            if (!await _UserSubscribtionRepository.AddUserSubscribtionAsync(userSubscribtionToAdd)
                ||!await _UserRepository.UpdateUserAsync(channel))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("unsubscribe-user-channel/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UnsubscribeToUserChannel(int userId)
        {
            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? subscriber = await _UserRepository.GetUserByNameAsync(username);

            if (subscriber is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            User? channel = await _UserRepository.GetUserAsync(userId);

            if (channel is null)
            {
                return BadRequest("This channel does not exist");
            }

            UserSubscribtion? userSubscribtion = await _UserSubscribtionRepository
                .GetUserSubscribtionByChannelAndSubscriberAsync(channel.Id, subscriber.Id);

            if (userSubscribtion is null)
            {
                return NotFound();
            }

            channel.SubscriberCount -= 1;

            if (!await _UserSubscribtionRepository.RemoveUserSubscribtionAsync(userSubscribtion)
                || !await _UserRepository.UpdateUserAsync(channel))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("user-like/{videoId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Expire3")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetVideoUserLike(int videoId)
        {
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


            VideoUserLikeDto? videoUserLike = _Mapper.Map<VideoUserLikeDto>(await _VideoUserLikeRepository
                .GetVideoUserLikeByVideoAndUserAsync(videoId, user.Id));

            if(videoUserLike is null)
            {
                return NotFound();
            }

            return Ok(videoUserLike);
        }

        [HttpPut("upvote/{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpvoteVideo([FromQuery] int upvoteValue, int videoId)
        {
            if (upvoteValue != 1 && upvoteValue != -1 && upvoteValue != 0)
            {
                return BadRequest(ModelState);
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

            Video? video = await _VideoRepository.GetVideoAsync(videoId);

            if (video is null)
            {
                return BadRequest("This video does not exist");
            }

            VideoUserLike? videoUserLike = await _VideoUserLikeRepository
                .GetVideoUserLikeByVideoAndUserAsync(video.Id, user.Id);

            if (videoUserLike is null)
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

            video.LikeCount += upvoteValue;
            videoUserLike.Rating = upvoteValue;

            if (!await _VideoRepository.UpdateVideoAsync(video) 
                || !await _VideoUserLikeRepository.UpdateVideoUserLikeAsync(videoUserLike))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
