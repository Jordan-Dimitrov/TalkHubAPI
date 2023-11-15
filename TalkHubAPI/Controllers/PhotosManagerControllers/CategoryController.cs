using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Controllers.PhotosManagerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        private readonly string _CategoriesCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;
        public CategoryController(IPhotoCategoryRepository photoCategoryRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _PhotoCategoryRepository = photoCategoryRepository;
            _MemoryCache= memoryCache;
            _CategoriesCacheKey = "photoCategories";
            _Mapper = mapper;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoCategoryDto>))]
        public async Task<IActionResult> GetCategories()
        {
            ICollection<PhotoCategoryDto>? categories = _MemoryCache.Get<List<PhotoCategoryDto>>(_CategoriesCacheKey);

            if (categories is null)
            {
                categories = _Mapper.Map<List<PhotoCategoryDto>>(await _PhotoCategoryRepository.GetCategoriesAsync());
                _MemoryCache.Set(_CategoriesCacheKey, categories, TimeSpan.FromMinutes(1));
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PhotoCategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            PhotoCategoryDto category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                .GetCategoryAsync(categoryId));

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] PhotoCategoryDto categoryCreate)
        {
            if (categoryCreate is null)
            {
                return BadRequest(ModelState);
            }

            if (await _PhotoCategoryRepository.PhotoCategoryExistsAsync(categoryCreate.CategoryName))
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PhotoCategory category = _Mapper.Map<PhotoCategory>(categoryCreate);

            if (!await _PhotoCategoryRepository.AddCategoryAsync(category))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_CategoriesCacheKey);

            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] PhotoCategoryDto updatedCategory)
        {
            if (updatedCategory is null || categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _PhotoCategoryRepository.CategoryExistsAsync(categoryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PhotoCategory categoryToUpdate = _Mapper.Map<PhotoCategory>(updatedCategory);

            if (!await _PhotoCategoryRepository.UpdateCategoryAsync(categoryToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_CategoriesCacheKey);

            return NoContent();
        }

        [HttpDelete("{categoryId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            PhotoCategory? categoryToDelete = await _PhotoCategoryRepository.GetCategoryAsync(categoryId);

            if (categoryToDelete is null)
            {
                return NotFound();
            }

            if (!await _PhotoCategoryRepository.RemoveCategoryAsync(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_CategoriesCacheKey);

            return NoContent();
        }

    }
}
