using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
