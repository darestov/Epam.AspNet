using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Epam.AspNet.Module1.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            using(var context = new NorthwindContext()) 
            {
                var products = context.Products.Include(x => x.Supplier).Include(x => x.Category).ToList();
                return View(products);
            }
        }
    }
}