using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        private readonly IMapper _Mapper;
        public CategoryController(IPhotoCategoryRepository photoCategoryRepository, IMapper mapper)
        {
            _PhotoCategoryRepository = photoCategoryRepository;
            _Mapper = mapper;
        }
        [HttpGet, Authorize(Roles = "User")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoCategoryDto>))]
        public IActionResult GetCategories()
        {
            ICollection<PhotoCategoryDto> categories = _Mapper.Map<List<PhotoCategoryDto>>(_PhotoCategoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}"), Authorize(Roles = "User")]
        [ProducesResponseType(200, Type = typeof(PhotoCategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_PhotoCategoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            PhotoCategoryDto category = _Mapper.Map<PhotoCategoryDto>(_PhotoCategoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }
        [HttpPost("createCategory"), Authorize(Roles = "User")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] PhotoCategoryDto categoryCreate)
        {
            if (categoryCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_PhotoCategoryRepository.PhotoCategoryExists(categoryCreate.PhotoName))
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PhotoCategory category = _Mapper.Map<PhotoCategory>(categoryCreate);

            if (!_PhotoCategoryRepository.AddCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("{categoryId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] PhotoCategoryDto updatedCategory)
        {
            if (updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if (categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_PhotoCategoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PhotoCategory categoryMap = _Mapper.Map<PhotoCategory>(updatedCategory);

            if (!_PhotoCategoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{categoryId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_PhotoCategoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            PhotoCategory categoryToDelete = _PhotoCategoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_PhotoCategoryRepository.RemoveCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }

    }
}
