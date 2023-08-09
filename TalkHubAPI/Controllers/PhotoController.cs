using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TalkHubAPI.Dto;
using System.IO;
using TalkHubAPI.Interfaces;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _PhotoRepository;
        private readonly IMapper _Mapper;
        public PhotoController(IPhotoRepository photoRepository, IMapper mapper)
        {
            _PhotoRepository = photoRepository;
            _Mapper = mapper;
        }
        private readonly string UploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
        [HttpPost]
        public IActionResult UploadMedia(IFormFile file)
        {
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(UploadsDirectory, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok("Media uploaded successfully.");
        }
        [HttpGet]
        [Route("api/media/{fileName}")]
        public IActionResult GetMedia(string fileName)
        {
            var filePath = Path.Combine(UploadsDirectory, fileName);

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return new FileContentResult(fileBytes, "image/png");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
