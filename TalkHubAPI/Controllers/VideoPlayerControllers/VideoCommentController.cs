﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Dto.VideoPlayerDtos;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repository.VideoPlayerRepositories;

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
        [Authorize]
        public async Task<IActionResult> CreateVideoComment(CreateVideoCommentDto commentDto)
        {
            if (commentDto == null)
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

            if (!await _VideoRepository.VideoExistsAsync(commentDto.VideoId))
            {
                return BadRequest("This video does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoComment comment = _Mapper.Map<VideoComment>(commentDto);
            comment.Video = _Mapper.Map<Video>(await _VideoRepository.GetVideoAsync(commentDto.VideoId));
            comment.User = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
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
        [ProducesResponseType(200, Type = typeof(VideoCommentDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVideoComment(int videoCommentId)
        {
            if (!await _VideoCommentRepository.VideoCommentExistsAsync(videoCommentId))
            {
                return NotFound();
            }

            VideoComment comment = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);

            VideoCommentDto commentDto = _Mapper.Map<VideoCommentDto>(await _VideoCommentRepository
                .GetVideoCommentAsync(videoCommentId));

            Video video = await _VideoRepository.GetVideoAsync(comment.VideoId);

            commentDto.User = _Mapper.Map<UserDto>(_UserRepository.GetUser(comment.UserId));
            commentDto.Video = _Mapper.Map<VideoDto>(await _VideoRepository.GetVideoAsync(comment.VideoId));

            commentDto.Video.Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository
                .GetVideoTagAsync(comment.Video.TagId));

            commentDto.Video.User = _Mapper.Map<UserDto>(_UserRepository.GetUser(video.UserId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(commentDto);
        }

        [HttpGet("videoCommentsByVideoId/{videoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoCommentDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllCommentsByVideo(int videoId)
        {
            if (!await _VideoRepository.VideoExistsAsync(videoId))
            {
                return BadRequest("This video does not exist");
            }

            List<VideoComment> comments = (await _VideoCommentRepository
                .GetVideoCommentsByVideoIdAsync(videoId)).ToList();

            List<VideoCommentDto> commentDtos = _Mapper
                .Map<List<VideoCommentDto>>(await _VideoCommentRepository
                .GetVideoCommentsByVideoIdAsync(videoId)).ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < comments.Count; i++)
            {
                Video video = await _VideoRepository.GetVideoAsync(comments[i].VideoId);

                commentDtos[i].Video = _Mapper.Map<VideoDto>(await _VideoRepository.GetVideoAsync(comments[i].VideoId));
                commentDtos[i].User = _Mapper.Map<UserDto>(_UserRepository.GetUser(comments[i].UserId));

                commentDtos[i].Video.Tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository
                    .GetVideoTagAsync(comments[i].Video.TagId));

                commentDtos[i].Video.User = _Mapper.Map<UserDto>(_UserRepository.GetUser(video.UserId));
            }

            return Ok(commentDtos);
        }

        [HttpPut("hide/{videoCommentId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideMessage(int videoCommentId)
        {
            if (!await _VideoRepository.VideoExistsAsync(videoCommentId))
            {
                return NotFound();
            }

            VideoComment commentToHide = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            commentToHide.MessageContent = "message was hidden";

            if (!await _VideoCommentRepository.UpdateVideoCommentAsync(commentToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("upvoteVideoComment/{videoCommentId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> UpvoteVideoComment([FromQuery] int upvoteValue, int videoCommentId)
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

            if (!await _VideoCommentRepository.VideoCommentExistsAsync(videoCommentId))
            {
                return BadRequest("This video does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoComment comment = await _VideoCommentRepository.GetVideoCommentAsync(videoCommentId);
            User user = _UserRepository.GetUserByName(username);

            if (!await _VideoCommentsLikeRepository.VideoCommentsLikeExistsForCommentAndUserAsync(comment.Id,user.Id))
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

            VideoCommentsLike videoCommentsLike = await _VideoCommentsLikeRepository
                .GetVideoCommentsLikeByCommentAndUserAsync(comment.Id, user.Id);

            if (upvoteValue == videoCommentsLike.Rating)
            {
                videoCommentsLike.Rating = 0;
                comment.LikeCount -= upvoteValue;
            }
            else
            {
                int temp = videoCommentsLike.Rating;
                videoCommentsLike.Rating = upvoteValue;
                comment.LikeCount += upvoteValue - temp;
            }

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
