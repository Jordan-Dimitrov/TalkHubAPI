using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        private readonly IMapper _Mapper;

        public VideoTagController(IVideoTagRepository videoTagRepository, IMapper mapper)
        {
            _VideoTagRepository = videoTagRepository;
            _Mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VideoTagDto>))]
        public async Task<IActionResult> GetTags()
        {
            ICollection<VideoTagDto> tags = _Mapper.Map<List<VideoTagDto>>(await _VideoTagRepository.GetVideoTagsAsync());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tags);
        }

        [HttpGet("{tagId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(VideoTagDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTag(int tagId)
        {
            if (!await _VideoTagRepository.VideoTagExistsAsync(tagId))
            {
                return NotFound();
            }

            VideoTagDto tag = _Mapper.Map<VideoTagDto>(await _VideoTagRepository.GetVideoTagAsync(tagId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(tag);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTag([FromBody] VideoTagDto tagCreate)
        {
            if (tagCreate == null)
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

            return Ok("Successfully created");
        }

        [HttpPut("{tagId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody] VideoTagDto updatedTag)
        {
            if (updatedTag == null || tagId != updatedTag.Id)
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

            return NoContent();
        }

        [HttpDelete("{tagId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            if (!await _VideoTagRepository.VideoTagExistsAsync(tagId))
            {
                return NotFound();
            }

            VideoTag tagToDelete = await _VideoTagRepository.GetVideoTagAsync(tagId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _VideoTagRepository.RemoveVideoTagAsync(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the tag");
            }

            return NoContent();
        }
    }
}
