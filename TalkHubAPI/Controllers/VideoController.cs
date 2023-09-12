using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers
{
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
