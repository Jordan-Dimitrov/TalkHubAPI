using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers
{
    public class PlaylistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
