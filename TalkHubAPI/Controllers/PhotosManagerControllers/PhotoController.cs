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
using System.Diagnostics;

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
        public async Task<IActionResult> UploadMedia(IFormFile file, [FromForm] CreatePhotoDto photoDto)
        {
            if(photoDto is null || !_FileProcessingService.ImageMimeTypeValid(file))
            {
                return BadRequest(ModelState);
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            PhotoCategory? category = await _PhotoCategoryRepository.GetCategoryAsync(photoDto.CategoryId);

            if (category is null)
            {
                return BadRequest("This category does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string response = await _FileProcessingService.UploadImageAsync(file);

            if (response == "File already exists")
            {
                return BadRequest(response);
            }

            string cacheKey = _PhotosCacheKey + $"_{photoDto.CategoryId}";

            Photo photo = _Mapper.Map<Photo>(photoDto);
            photo.Category = category;

            photo.FileName = response;
            photo.Timestamp = DateTime.Now;
            photo.User = user;

            if (!await _PhotoRepository.AddPhotoAsync(photo))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return Ok("Successfully created");
        }

        [HttpGet("{photoId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(PhotoDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            Photo? photo = await _PhotoRepository.GetPhotoAsync(photoId);

            if (photo is null)
            {
                return NotFound();
            }

            PhotoDto photoDto = _Mapper.Map<PhotoDto>(photo);
            photoDto.Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                .GetCategoryAsync(photo.CategoryId));

            photoDto.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photo.UserId));

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

            if (file is null)
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
            List<Photo> photos = (await _PhotoRepository.GetPhotosAsync()).ToList();
            List<PhotoDto> photoDtos = _Mapper.Map<List<PhotoDto>>(photos);

            for (int i = 0; i < photos.Count; i++)
            {
                photoDtos[i].Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                    .GetCategoryAsync(photos[i].CategoryId));

                photoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(photos[i].UserId));
            }

            return Ok(photoDtos);
        }

        [HttpGet("category/{categoryId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMediaByCategory(int categoryId)
        {
            PhotoCategory? category = await _PhotoCategoryRepository.GetCategoryAsync(categoryId);

            if (category is null)
            {
                return BadRequest("This category does not exist");
            }

            string cacheKey = _PhotosCacheKey + $"_{categoryId}";
            List<PhotoDto>? photoDtos = _MemoryCache.Get<List<PhotoDto>>(cacheKey);

            if (photoDtos is null)
            {
                List<Photo> photos = (await _PhotoRepository.GetPhotosByCategoryIdAsync(categoryId)).ToList();
                photoDtos = _Mapper.Map<List<PhotoDto>>(photos);

                for (int i = 0; i < photos.Count; i++)
                {
                    photoDtos[i].Category = _Mapper.Map<PhotoCategoryDto>(category);

                    photoDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository
                        .GetUserAsync(photos[i].UserId));
                }

                _MemoryCache.Set(cacheKey, photoDtos, TimeSpan.FromMinutes(1));
            }

            return Ok(photoDtos);
        }

        [HttpGet("user/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMediaByUser(int userId)
        {
            User? user = await _UserRepository.GetUserAsync(userId);

            if (user is null)
            {
                return BadRequest("This user does not exist");
            }

            List<Photo> photos = (await _PhotoRepository.GetPhotosByUserIdAsync(userId)).ToList();
            List<PhotoDto> photosDto = _Mapper.Map<List<PhotoDto>>(photos);

            for (int i = 0; i < photos.Count; i++)
            {
                photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(await _PhotoCategoryRepository
                    .GetCategoryAsync(photos[i].CategoryId));
                photosDto[i].User = _Mapper.Map<UserDto>(user);
            }

            return Ok(photosDto);
        }

        [HttpDelete("{photoId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            Photo? photoToDelete = await _PhotoRepository.GetPhotoAsync(photoId);

            if (photoToDelete is null)
            {
                return NotFound();
            }

            string cacheKey = _PhotosCacheKey + $"_{photoToDelete.CategoryId}";

            if (!await _FileProcessingService.RemoveMediaAsync(photoToDelete.FileName))
            {
                return BadRequest("Unexpected error");
            }

            if (!await _PhotoRepository.RemovePhotoAsync(photoToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }
    }
}
