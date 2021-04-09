using Microsoft.AspNetCore.Mvc;

namespace FireCloud.UserService.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
