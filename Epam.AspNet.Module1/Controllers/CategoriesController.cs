using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Epam.AspNet.Module1.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new NorthwindContext()) 
            {
                List<Category> categories = context.Categories.ToList();
                return View(categories);
            }
        }
    }
}