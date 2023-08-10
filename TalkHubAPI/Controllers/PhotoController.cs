using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TalkHubAPI.Dto;
using System.IO;
using TalkHubAPI.Interfaces;
using Microsoft.AspNetCore.Cors;
using Azure.Core;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TalkHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _PhotoRepository;
        private readonly IMapper _Mapper;
        private readonly IFileProcessingService _FileProcessingService;
        public PhotoController(IPhotoRepository photoRepository, IMapper mapper, IFileProcessingService fileProcessingService)
        {
            _PhotoRepository = photoRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
        }
        [HttpPost]
        public IActionResult UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ModelState);
            }
            string response = _FileProcessingService.UploadMedia(file);
            return Ok(response);
        }
        [HttpGet("{fileName}")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
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
    }
}
