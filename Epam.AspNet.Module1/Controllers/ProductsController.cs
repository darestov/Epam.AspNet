using System.Linq;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
                return NotFound();
            return EditProductInternal(product);
        }

        private IActionResult EditProductInternal(Product product)
        {
            ViewBag.Categories = context.Categories.Select(c => new SelectListItem(c.CategoryName, c.CategoryID.ToString())).ToList();
            ViewBag.Suppliers = context.Suppliers.Select(s => new SelectListItem(s.CompanyName, s.SupplierID.ToString())).ToList();
            return View(nameof(Edit), product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var oldProduct = context.Products.Find(product.ProductID);
            if(oldProduct!=null)
            {
                // we're updating existing product
                if(TryUpdateModelAsync(oldProduct).Result)
                {
                    context.SaveChanges();
                    TempData["SaveMessage"] = "Successfully saved";
                    return RedirectToAction(nameof(Edit), new { id = product.ProductID });
                }
            }
            else
            {
                // a new product
                context.Products.Add(product);
                context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult New()
        {
            return EditProductInternal(new Product());
        }
    }
}