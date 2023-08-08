using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public IActionResult Test(IFormFile file)
        {
            return Ok(file.FileName);
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoCategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _Mapper.Map<List<PhotoCategoryDto>>(_PhotoCategoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(PhotoCategory))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_PhotoCategoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = _Mapper.Map<PhotoCategoryDto>(_PhotoCategoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }
    }
}
