using Microsoft.AspNetCore.Mvc;

namespace Epam.AspNet.Module1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}