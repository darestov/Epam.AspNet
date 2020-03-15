using System.Linq;
using Epam.AspNet.Module1.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Epam.AspNet.Module1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext context;

        public CategoriesController(NorthwindContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Categories.ToList());
        }
    }
}