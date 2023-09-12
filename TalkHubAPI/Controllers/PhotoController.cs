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

namespace TalkHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _PhotoRepository;
        private readonly IMapper _Mapper;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserRepository _UserRepository;
        private readonly IAuthService _AuthService;
        private readonly IPhotoCategoryRepository _PhotoCategoryRepository;
        public PhotoController(IPhotoRepository photoRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IUserRepository userRepository,
            IAuthService authService,
            IPhotoCategoryRepository photoCategoryRepository)
        {
            _PhotoRepository = photoRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _UserRepository = userRepository;
            _AuthService = authService;
            _PhotoCategoryRepository = photoCategoryRepository;
        }
        [HttpPost("addMedia"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult UploadMedia(IFormFile file, [FromForm] CreatePhotoDto photoDto)
        {
            if (file == null || file.Length == 0 || photoDto == null)
            {
                return BadRequest(ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!_PhotoCategoryRepository.CategoryExists(photoDto.CategoryId))
            {
                return BadRequest("This category does not exist");
            }

            string response = _FileProcessingService.UploadMedia(file);

            if (response == "Empty" || response == "Invalid file format" || response == "File already exists")
            {
                return BadRequest(response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Photo photo = _Mapper.Map<Photo>(photoDto);
            photo.Category = _Mapper.Map<PhotoCategory>(_PhotoCategoryRepository.GetCategory(photo.CategoryId));

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            photo.FileName = file.FileName;
            photo.Timestamp = DateTime.Now;
            photo.User = user;

            if (!_PhotoRepository.AddPhoto(photo))
            {
                return BadRequest(ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpGet("{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetMedia(string fileName)
        {
            FileContentResult file = _FileProcessingService.GetMedia(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }
        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetAllMedia()
        {
            List<Photo> photos = _PhotoRepository.GetPhotos().ToList();
            List<PhotoDto> photosDto = _Mapper.Map<List<PhotoDto>>(_PhotoRepository.GetPhotos());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < photos.Count; i++)
            {
                photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(_PhotoCategoryRepository.GetCategory(photos[i].CategoryId));
            }

            return Ok(photosDto);
        }
        [HttpGet("category/{categoryId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetAllMediaByCategory(int categoryId)
        {

            if (!_PhotoCategoryRepository.CategoryExists(categoryId))
            {
                return BadRequest("This category does not exist");
            }

            List<Photo> photos = _PhotoRepository.GetPhotosByCategoryId(categoryId).ToList();
            List<PhotoDto> photosDto = _Mapper.Map<List<PhotoDto>>(_PhotoRepository.GetPhotosByCategoryId(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < photos.Count; i++)
            {
                photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(_PhotoCategoryRepository.GetCategory(photos[i].CategoryId));
                photosDto[i].User = _Mapper.Map<UserDto>(_UserRepository.GetUser(photos[i].UserId));
            }

            return Ok(photosDto);
        }
        [HttpGet("user/{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetAllMediaByUser(int userId)
        {

            if (!_UserRepository.UserExists(userId))
            {
                return BadRequest("This user does not exist");
            }

            List<Photo> photos = _PhotoRepository.GetPhotosByUserId(userId).ToList();
            List<PhotoDto> photosDto = _Mapper.Map<List<PhotoDto>>(_PhotoRepository.GetPhotosByUserId(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < photos.Count; i++)
            {
                photosDto[i].Category = _Mapper.Map<PhotoCategoryDto>(_PhotoCategoryRepository.GetCategory(photos[i].CategoryId));
                photosDto[i].User = _Mapper.Map<UserDto>(_UserRepository.GetUser(photos[i].UserId));
            }

            return Ok(photosDto);
        }
        [HttpDelete("{photoId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePhoto(int photoId)
        {
            if (!_PhotoRepository.PhotoExists(photoId))
            {
                return NotFound();
            }

            Photo photoToDelete = _PhotoRepository.GetPhoto(photoId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_FileProcessingService.RemoveMedia(photoToDelete.FileName))
            {
                return BadRequest("Unexpected error");
            }

            if (!_PhotoRepository.RemovePhoto(photoToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
