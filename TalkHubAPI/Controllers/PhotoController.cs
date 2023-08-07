using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Dto;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : Controller
    {
        [HttpPost]
        public IActionResult Test(CreatePhotoDto request)
        {
            return Ok(request);
        }
    }
}
