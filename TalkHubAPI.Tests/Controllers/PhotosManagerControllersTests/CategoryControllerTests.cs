using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TalkHubAPI.Controllers.PhotosManagerControllers;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.ConfigurationModels;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Tests.Controller.PhotosManagerControllersTests
{
    public class CategoryControllerTests
    {
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;
        private readonly IOptions<MemoryCacheSettings> _MemoryCacheSettings;
        public CategoryControllerTests()
        {
            _PhotoCategoryRepository = A.Fake<IPhotoCategoryRepository>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _Mapper = A.Fake<IMapper>();
            _MemoryCacheSettings = A.Fake<IOptions<MemoryCacheSettings>>();
        }
        [Fact]
        public async Task CategoryController_GetCategory_ReturnOk()
        {
            int categoryId = 1;
            PhotoCategoryDto categoryDto = A.Fake<PhotoCategoryDto>();
            PhotoCategory photoCategory = A.Fake<PhotoCategory>();

            A.CallTo(() => _PhotoCategoryRepository.GetCategoryAsync(categoryId)).Returns(photoCategory);
            CategoryController controller = new CategoryController(_PhotoCategoryRepository,
                _Mapper, _MemoryCache, _MemoryCacheSettings);

            IActionResult result = await controller.GetCategory(categoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
