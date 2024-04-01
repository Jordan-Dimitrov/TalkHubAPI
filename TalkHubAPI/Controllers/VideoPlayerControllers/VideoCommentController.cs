using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Dtos.UserDtos;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoCommentController : Controller
    {
        private readonly IVideoCommentRepository _VideoCommentRepository;
        private readonly IMapper _Mapper;
        private readonly IUserRepository _UserRepository;
        private readonly IAuthService _AuthService;
        private readonly IVideoRepository _VideoRepository;
        private readonly IVideoCommentsLikeRepository _VideoCommentsLikeRepository;
        private readonly IVideoTagRepository _VideoTagRepository;
        public VideoCommentController(IVideoCommentRepository videoCommentRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IAuthService authService,
            IVideoRepository videoRepository,
            IVideoCommentsLikeRepository videoCommentsLikeRepository,
            IVideoTagRepository videoTagRepository)
        {
            _VideoCommentRepository = videoCommentRepository;
            _Mapper = mapper;
            _UserRepository = userRepository;
            _AuthService = authService;
            _VideoRepository = videoRepository;
            _VideoCommentsLikeRepository = videoCommentsLikeRepository;
            _VideoTagRepository = videoTagRepository;
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateVideoComment(CreateVideoCommentDto commentDto)
        {
            if (commentDto is null)
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

            Video? video = await _VideoRepository.GetVideoAsync(commentDto.VideoId);

            if (video is null)
            {
                return BadRequest("This video does not exist");
            }

            if (commentDto.ReplyId is not null)
            {
                VideoComment? parent = await _VideoCommentRepository.GetVideoCommentAsync(commentDto.ReplyId ?? -1);

                if (parent is null || parent.VideoId != commentDto.VideoId)
                {
                    return BadRequest("Invalid reply");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoComment comment = _Mapper.Map<VideoComment>(commentDto);
            comment.Video = video;
            comment.User = user;
            comment.DateCreated = DateTime.Now;
            comment.LikeCount = 0;

            if (!await _VideoCommentRepository.AddVideoCommentAsync(comment))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("{videoCommentId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(VideoCommentDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVideoComment(int videoCommentId)
        {

            VideoComment? comment = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);

            if (comment is null)
            {
                return NotFound();
            }

            VideoCommentDto commentDto = _Mapper.Map<VideoCommentDto>(comment);

            Video? video = await _VideoRepository.GetVideoAsync(comment.VideoId);

            commentDto.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(comment.UserId));
            commentDto.Video = _Mapper.Map<VideoDto>(await _VideoRepository.GetVideoAsync(comment.VideoId));

            commentDto.Video.Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository
                .GetVideoTagAsync(comment.Video.TagId));

            commentDto.Video.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(video.UserId));

            return Ok(commentDto);
        }

        [HttpGet("video/{videoId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoCommentDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllCommentsByVideo(int videoId)
        {
            Video? video = await _VideoRepository.GetVideoAsync(videoId);

            if (video is null)
            {
                return BadRequest("This video does not exist");
            }

            List<VideoComment> comments = (await _VideoCommentRepository
                .GetVideoCommentsByVideoIdAsync(videoId)).ToList();

            List<VideoCommentDto> commentDtos = _Mapper.Map<List<VideoCommentDto>>(comments);

            for (int i = 0; i < comments.Count; i++)
            {

                commentDtos[i].Video = _Mapper.Map<VideoDto>(video);
                commentDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(comments[i].UserId));

                commentDtos[i].Video.Tag = _Mapper.Map<VideoTagDto>(video.Tag);

                commentDtos[i].Video.User = _Mapper.Map<UserDto>(video.User);
            }

            return Ok(commentDtos);
        }

        [HttpPut("hide/{videoCommentId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideMessage(int videoCommentId)
        {

            VideoComment? commentToHide = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);

            if (commentToHide is null)
            {
                return NotFound();
            }

            commentToHide.MessageContent = "message was hidden";

            if (!await _VideoCommentRepository.UpdateVideoCommentAsync(commentToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("video-comment-like/{videoCommentId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Expire3")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetVideoCommentLike(int videoCommentId)
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

            VideoCommentsLikeDto? videoComment = _Mapper.Map<VideoCommentsLikeDto>(await _VideoCommentsLikeRepository
                .GetVideoCommentsLikeByCommentAndUserAsync(videoCommentId, user.Id));

            if (videoComment is null)
            {
                return NotFound();
            }

            return Ok(videoComment);
        }

        [HttpPut("upvote/{videoCommentId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpvoteVideoComment([FromQuery] int upvoteValue, int videoCommentId)
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

            VideoComment? comment = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);

            if (comment is null)
            {
                return BadRequest("This video does not exist");
            }

            VideoCommentsLike? videoCommentsLike = await _VideoCommentsLikeRepository
                .GetVideoCommentsLikeByCommentAndUserAsync(comment.Id, user.Id);

            if (videoCommentsLike is null)
            {
                VideoCommentsLike videoCommentsLikeToAdd = new VideoCommentsLike();
                videoCommentsLikeToAdd.Rating = upvoteValue;
                videoCommentsLikeToAdd.VideoComment = comment;
                videoCommentsLikeToAdd.User = user;
                comment.LikeCount += upvoteValue;

                if (!await _VideoCommentsLikeRepository.AddVideoCommentsLikeAsync(videoCommentsLikeToAdd))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }

            if (videoCommentsLike.Rating == upvoteValue)
            {
                return NoContent();
            }

            comment.LikeCount -= videoCommentsLike.Rating;
            comment.LikeCount += upvoteValue;
            videoCommentsLike.Rating = upvoteValue;

            if (!await _VideoCommentRepository.UpdateVideoCommentAsync(comment) ||
                !await _VideoCommentsLikeRepository.UpdateVideoCommentsLikeAsync(videoCommentsLike))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
