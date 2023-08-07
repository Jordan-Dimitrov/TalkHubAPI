using Microsoft.AspNetCore.Mvc;

namespace TalkHubAPI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return Ok(1);
        }
    }
}
