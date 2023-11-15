using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Controllers.ForumControllers;
using TalkHubAPI.Controllers.PhotosManagerControllers;
using TalkHubAPI.Dtos.ForumDtos;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Tests.Controller.PhotosManagerControllersTests
{
    public class CategoryControllerTests
    {
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;
        public CategoryControllerTests()
        {
            _PhotoCategoryRepository = A.Fake<IPhotoCategoryRepository>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _Mapper = A.Fake<IMapper>();
        }
        [Fact]
        public async Task CategoryController_GetCategory_ReturnOk()
        {
            int categoryId = 1;
            PhotoCategoryDto categoryDto = A.Fake<PhotoCategoryDto>();
            PhotoCategory photoCategory = A.Fake<PhotoCategory>();

            A.CallTo(() => _PhotoCategoryRepository.GetCategoryAsync(categoryId)).Returns(photoCategory);
            CategoryController controller = new CategoryController(_PhotoCategoryRepository,
                _Mapper, _MemoryCache);

            IActionResult result = await controller.GetCategory(categoryId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
