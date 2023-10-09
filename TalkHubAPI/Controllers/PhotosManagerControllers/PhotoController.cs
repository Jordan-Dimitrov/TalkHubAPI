using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using TalkHubAPI.Interfaces;
using Microsoft.AspNetCore.Cors;
using Azure.Core;
using System;
using Microsoft.AspNetCore.Authorization;
using TalkHubAPI.Models;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Dto.PhotosDtos;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Models.PhotosManagerModels;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace TalkHubAPI.Controllers.PhotosManagerControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _PhotoRepository;
        private readonly IMapper _Mapper;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserRepository _UserRepository;
        private readonly string _PhotosCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IAuthService _AuthService;
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        public PhotoController(IPhotoRepository photoRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IUserRepository userRepository,
            IAuthService authService,
            IPhotoCategoryRepository photoCategoryRepository,
            IMemoryCache memoryCache)
        {
            _PhotoRepository = photoRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _UserRepository = userRepository;
            _AuthService = authService;
            _PhotoCategoryRepository = photoCategoryRepository;
            _MemoryCache = memoryCache;
            _PhotosCacheKey = "photos";
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> UploadMedia(IFormFile file, [FromForm] CreatePhotoDto photoDto)
        {
            if (file == null || file.Length == 0 || photoDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_FileProcessingService.GetContentType(file.FileName) == "video/mp4")
            {
                return BadRequest(ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _PhotoCategoryRepository.CategoryExistsAsync(photoDto.CategoryId))
            {
                return BadRequest("This category does not exist");
            }

            string response = await _FileProcessingService.UploadImageAsync(file);

            if (response == "Empty" || response == "Invalid file format" || response == "File already exists")
            {
                return BadRequest(response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string cacheKey = _PhotosCacheKey + $"_{photoDto.CategoryId}";

            Photo photo = _Mapper.Map<Photo>(photoDto);
            photo.Category = _Mapper.Map<PhotoCategory>(await _PhotoCategoryRepository.GetCategoryAsync(photo.CategoryId));

            User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(username));
            photo.FileName = response;
            photo.Timestamp = DateTime.Now;
            photo.User = user;

            if (!await _PhotoRepository.AddPhotoAsync(photo))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_PhotosCacheKey);
            _MemoryCache.Remove(cacheKey);

            return Ok("Successfully created");
        }

        [HttpGet("{photoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PhotoDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            if (!await _PhotoRepository.PhotoExistsAsync(photoId))
            {
                return NotFound();
            }

            Photo photo = await _PhotoRepository.GetPhotoAsync(photoId);
            PhotoDto photoDto = _Mapper.Map<PhotoDto>(await _PhotoRepository.GetPhotoAsync(photoId));
            photoDto.Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository.GetCategoryAsync(photo.CategoryId));
            photoDto.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photo.UserId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(photoDto);
        }

        [HttpGet("file/{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetMedia(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) != "image/webp")
            {
                return BadRequest(ModelState);
            }

            FileContentResult file = await _FileProcessingService.GetImageAsync(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMedia()
        {
            List<PhotoDto> photosDto = _MemoryCache.Get<List<PhotoDto>>(_PhotosCacheKey);

            if (photosDto == null)
            {
                photosDto = _Mapper.Map<List<PhotoDto>>(await _PhotoRepository.GetPhotosAsync());
                List<Photo> photos = (await _PhotoRepository.GetPhotosAsync()).ToList();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                for (int i = 0; i < photos.Count; i++)
                {
                    photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                        .GetCategoryAsync(photos[i].CategoryId));
                    photosDto[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photos[i].UserId));
                }

                _MemoryCache.Set(_PhotosCacheKey, photosDto, TimeSpan.FromMinutes(1));
            }

            return Ok(photosDto);
        }

        [HttpGet("category/{categoryId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMediaByCategory(int categoryId)
        {

            if (!await _PhotoCategoryRepository.CategoryExistsAsync(categoryId))
            {
                return BadRequest("This category does not exist");
            }
            string cacheKey = _PhotosCacheKey + $"_{categoryId}";
            List<PhotoDto> photosDto = _MemoryCache.Get<List<PhotoDto>>(cacheKey);

            if (photosDto == null)
            {
                photosDto = _Mapper.Map<List<PhotoDto>>(await _PhotoRepository.GetPhotosByCategoryIdAsync(categoryId));

                List<Photo> photos = (await _PhotoRepository.GetPhotosByCategoryIdAsync(categoryId)).ToList();

                for (int i = 0; i < photos.Count; i++)
                {
                    photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                        .GetCategoryAsync(photos[i].CategoryId));

                    photosDto[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photos[i].UserId));
                }

                _MemoryCache.Set(cacheKey, photosDto, TimeSpan.FromMinutes(1));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(photosDto);
        }

        [HttpGet("user/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMediaByUser(int userId)
        {

            if (!await _UserRepository.UserExistsAsync(userId))
            {
                return BadRequest("This user does not exist");
            }

            List<Photo> photos = (await _PhotoRepository.GetPhotosByUserIdAsync(userId)).ToList();
            List<PhotoDto> photosDto = _Mapper.Map<List<PhotoDto>>(await _PhotoRepository.GetPhotosByUserIdAsync(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < photos.Count; i++)
            {
                photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                    .GetCategoryAsync(photos[i].CategoryId));
                photosDto[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photos[i].UserId));
            }

            return Ok(photosDto);
        }

        [HttpDelete("{photoId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            if (!await _PhotoRepository.PhotoExistsAsync(photoId))
            {
                return NotFound();
            }

            Photo photoToDelete = await _PhotoRepository.GetPhotoAsync(photoId);
            string cacheKey = _PhotosCacheKey + $"_{photoToDelete.CategoryId}";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _FileProcessingService.RemoveMediaAsync(photoToDelete.FileName))
            {
                return BadRequest("Unexpected error");
            }

            if (!await _PhotoRepository.RemovePhotoAsync(photoToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            _MemoryCache.Remove(_PhotosCacheKey);
            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }
    }
}
