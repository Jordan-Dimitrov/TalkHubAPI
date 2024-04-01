using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Controllers.PhotosManagerControllers;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Tests.Controller.PhotosManagerControllersTests
{
    public class PhotoControllerTests
    {
        private readonly IPhotoRepository _PhotoRepository;
        private readonly IMapper _Mapper;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserRepository _UserRepository;
        private readonly IMemoryCache _MemoryCache;
        private readonly IAuthService _AuthService;
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        public PhotoControllerTests()
        {
            _Mapper = A.Fake<IMapper>();
            _FileProcessingService = A.Fake<IFileProcessingService>();
            _UserRepository = A.Fake<IUserRepository>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _AuthService = A.Fake<IAuthService>();
            _PhotoRepository = A.Fake<IPhotoRepository>();
            _PhotoCategoryRepository = A.Fake<IPhotoCategoryRepository>();
        }
        [Fact]
        public async Task PhotoController_GetPhoto_ReturnOk()
        {
            int photoId = 1;
            int categoryId = 1;
            PhotoDto photoDto = A.Fake<PhotoDto>();
            Photo photo = A.Fake<Photo>();
            PhotoCategory category = A.Fake<PhotoCategory>();

            A.CallTo(() => _PhotoRepository.GetPhotoAsync(photoId)).Returns(photo);
            A.CallTo(() => _PhotoCategoryRepository.GetCategoryAsync(categoryId)).Returns(category);
            PhotoController controller = new PhotoController(_PhotoRepository,
                _Mapper, _FileProcessingService,
                _UserRepository, _AuthService,
                _PhotoCategoryRepository, _MemoryCache);

            IActionResult result = await controller.GetPhoto(photoId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
