using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers
{
    public class ForumMessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
