using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return Content("Area: Main, Controller: Home, Action: Privacy");
        }
        public IActionResult Terms()
        {
            return Content("Area: Main, Controller: Home, Action: Terms & Condition");
        }
        public IActionResult Contact()
        {
            return Content("Area: Main, Controller: Home, Action: Contact");
        }
        public IActionResult Cookies()
        {
            return Content("Area: Main, Controller: Home, Action: Cookie Policies");
        }
    }
}
