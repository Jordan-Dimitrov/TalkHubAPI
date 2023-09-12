using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers
{
    public class VideoCommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
