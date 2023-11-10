using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Threading;
using TalkHubAPI.Dto.VideoPlayerDtos;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoTagController : Controller
    {
        private readonly IVideoTagRepository _VideoTagRepository;
        private readonly string _VideoTagsCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;

        public VideoTagController(IVideoTagRepository videoTagRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _VideoTagRepository = videoTagRepository;
            _MemoryCache = memoryCache;
            _VideoTagsCacheKey = "videoTags";
            _Mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoTagDto>))]
        public async Task<IActionResult> GetTags()
        {
            ICollection<VideoTagDto>? tags = _MemoryCache.Get<List<VideoTagDto>>(_VideoTagsCacheKey);

            if (tags is null)
            {
                tags = _Mapper.Map<List<VideoTagDto>>(await _VideoTagRepository.GetVideoTagsAsync());

                _MemoryCache.Set(_VideoTagsCacheKey, tags, TimeSpan.FromMinutes(1));
            }

            return Ok(tags);
        }

        [HttpGet("{tagId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(VideoTagDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTag(int tagId)
        {
            VideoTagDto tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(tagId));

            if (tag is null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTag([FromBody] VideoTagDto tagCreate)
        {
            if (tagCreate is null)
            {
                return BadRequest(ModelState);
            }

            if (await _VideoTagRepository.VideoTagExistsAsync(tagCreate.TagName))
            {
                ModelState.AddModelError("", "Tag already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoTag tag = _Mapper.Map<VideoTag>(tagCreate);

            if (!await _VideoTagRepository.AddVideoTagAsync(tag))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_VideoTagsCacheKey);

            return Ok("Successfully created");
        }

        [HttpPut("{tagId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody] VideoTagDto updatedTag)
        {
            if (updatedTag is null || tagId != updatedTag.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _VideoTagRepository.VideoTagExistsAsync(tagId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            VideoTag tagToUpdate = _Mapper.Map<VideoTag>(updatedTag);

            if (!await _VideoTagRepository.UpdateVideoTagAsync(tagToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the tag");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_VideoTagsCacheKey);

            return NoContent();
        }

        [HttpDelete("{tagId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            VideoTag? tagToDelete = await _VideoTagRepository.GetVideoTagAsync(tagId);

            if (tagToDelete is null)
            {
                return NotFound();
            }

            if (!await _VideoTagRepository.RemoveVideoTagAsync(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the tag");
            }

            _MemoryCache.Remove(_VideoTagsCacheKey);

            return NoContent();
        }
    }
}
