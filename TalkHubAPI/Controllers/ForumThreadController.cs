using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Dto;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumThreadController : Controller
    {
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IMapper _Mapper;

        public ForumThreadController(IForumThreadRepository forumThreadRepository, IMapper mapper)
        {
            _ForumThreadRepository = forumThreadRepository;
            _Mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ForumThreadDto>))]
        public IActionResult GetThreads()
        {
            ICollection<ForumThreadDto> threads = _Mapper.Map<List<ForumThreadDto>>(_ForumThreadRepository.GetForumThreads());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(threads);
        }

        [HttpGet("{threadId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(ForumThreadDto))]
        [ProducesResponseType(400)]
        public IActionResult GetThread(int threadId)
        {
            if (!_ForumThreadRepository.ForumThreadExists(threadId))
            {
                return NotFound();
            }

            ForumThreadDto thread = _Mapper.Map<ForumThreadDto>(_ForumThreadRepository.GetForumThread(threadId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(thread);
        }

        [HttpPost("createThread"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateThread([FromBody] ForumThreadDto threadCreate)
        {
            if (threadCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_ForumThreadRepository.ForumThreadExists(threadCreate.ThreadName))
            {
                ModelState.AddModelError("", "Thread already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumThread thread = _Mapper.Map<ForumThread>(threadCreate);

            if (!_ForumThreadRepository.AddForumThread(thread))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{threadId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateThread(int threadId, [FromBody] ForumThreadDto updatedThread)
        {
            if (updatedThread == null || threadId != updatedThread.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_ForumThreadRepository.ForumThreadExists(threadId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ForumThread threadMap = _Mapper.Map<ForumThread>(updatedThread);

            if (!_ForumThreadRepository.UpdateForumThread(threadMap))
            {
                ModelState.AddModelError("", "Something went wrong updating the thread");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{threadId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteThread(int threadId)
        {
            if (!_ForumThreadRepository.ForumThreadExists(threadId))
            {
                return NotFound();
            }

            ForumThread threadToDelete = _ForumThreadRepository.GetForumThread(threadId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ForumThreadRepository.RemoveForumThread(threadToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the thread");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
