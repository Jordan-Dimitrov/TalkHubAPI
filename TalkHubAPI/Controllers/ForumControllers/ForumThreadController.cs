using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using TalkHubAPI.Dto.ForumDtos;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Controllers.ForumControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumThreadController : Controller
    {
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly string _ThreadsCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;
        public ForumThreadController(IForumThreadRepository forumThreadRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _ForumThreadRepository = forumThreadRepository;
            _Mapper = mapper;
            _MemoryCache = memoryCache;
            _ThreadsCacheKey = "forumThreads";
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ForumThreadDto>))]
        public async Task<IActionResult> GetThreads()
        {
            ICollection<ForumThreadDto> threads = _MemoryCache.Get<List<ForumThreadDto>>(_ThreadsCacheKey);
            
            if (threads == null)
            {
                threads = _Mapper.Map<List<ForumThreadDto>>(await _ForumThreadRepository.GetForumThreadsAsync());

                _MemoryCache.Set(_ThreadsCacheKey, threads, TimeSpan.FromMinutes(1));
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(threads);
        }

        [HttpGet("{threadId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(ForumThreadDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetThread(int threadId)
        {
            if (!await _ForumThreadRepository.ForumThreadExistsAsync(threadId))
            {
                return NotFound();
            }

            ForumThreadDto thread = _Mapper.Map<ForumThreadDto>(await _ForumThreadRepository.GetForumThreadAsync(threadId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(thread);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateThread([FromBody] ForumThreadDto threadCreate)
        {
            if (threadCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (await _ForumThreadRepository.ForumThreadExistsAsync(threadCreate.ThreadName))
            {
                ModelState.AddModelError("", "Thread already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumThread thread = _Mapper.Map<ForumThread>(threadCreate);

            if (!await _ForumThreadRepository.AddForumThreadAsync(thread))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_ThreadsCacheKey);

            return Ok("Successfully created");
        }

        [HttpPut("{threadId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateThread(int threadId, [FromBody] ForumThreadDto updatedThread)
        {
            if (updatedThread == null || threadId != updatedThread.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _ForumThreadRepository.ForumThreadExistsAsync(threadId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ForumThread threadToUpdate = _Mapper.Map<ForumThread>(updatedThread);

            if (!await _ForumThreadRepository.UpdateForumThreadAsync(threadToUpdate))
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
        public async Task<IActionResult> DeleteThread(int threadId)
        {
            if (!await _ForumThreadRepository.ForumThreadExistsAsync(threadId))
            {
                return NotFound();
            }

            ForumThread threadToDelete = await _ForumThreadRepository.GetForumThreadAsync(threadId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _ForumThreadRepository.RemoveForumThreadAsync(threadToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the thread");
            }

            _MemoryCache.Remove(_ThreadsCacheKey);

            return NoContent();
        }
    }
}
