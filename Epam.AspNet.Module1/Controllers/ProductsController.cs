using System.Linq;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Epam.AspNet.Module1.Controllers
{
    public class ProductsController : Controller
    {  
        private readonly NorthwindContext context;
        public ProductsController(NorthwindContext context, IConfiguration config)
        {
            Config = config;
            this.context = context;
        }

        public IConfiguration Config { get; }


        public IActionResult Index()
        {
            int max = Config.GetValue("AppSettings:MaxProducts", 0);
            IQueryable<Product> products = context.Products.Include(x => x.Supplier).Include(x => x.Category);
            if (max != 0)
                products = products.Take(max);
            return View(products.ToList());
        }
    }
}