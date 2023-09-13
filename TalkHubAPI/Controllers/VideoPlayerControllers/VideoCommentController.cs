using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers.VideoPlayerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoCommentController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
