using System.Linq;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Epam.AspNet.Module1.Controllers
{
    public class ProductsController : Controller
    {  
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductsController(IConfiguration config, IUnitOfWork unitOfWork, IProductRepository productRepository, ISupplierRepository supplierRepository, ICategoryRepository categoryRepository)
        {
            Config = config;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.supplierRepository = supplierRepository;
            this.categoryRepository = categoryRepository;
        }

        public IConfiguration Config { get; }


        public IActionResult Index()
        {
            int max = Config.GetValue("AppSettings:MaxProducts", 0);
            return View(productRepository.ListProductsWithCategoriesAndSuppliers(max));
        }

        public IActionResult Edit(int id)
        {
            var product = productRepository.GetProduct(id);
            if (product == null)
                return NotFound();
            return EditProductInternal(product);
        }

        private IActionResult EditProductInternal(Product product)
        {
            ViewBag.CategoryID = categoryRepository.ListCategories().Select(c => new SelectListItem(c.CategoryName, c.CategoryID.ToString())).ToList();
            ViewBag.SupplierID = supplierRepository.ListSuppliers().Select(s => new SelectListItem(s.CompanyName, s.SupplierID.ToString())).ToList();
            return View(nameof(Edit), product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (product.UnitPrice > 1000)
            {
                ModelState.AddModelError(nameof(Product.UnitPrice), "That's too much!");
            }

            if(!ModelState.IsValid)
            {
                return EditProductInternal(product);
            }

            var oldProduct = productRepository.GetProduct(product.ProductID);
            if(oldProduct!=null)
            {
                // we're updating existing product
                if(TryUpdateModelAsync(oldProduct).Result)
                {
                    unitOfWork.SaveChanges();
                    TempData["SaveMessage"] = "Successfully saved";
                    return RedirectToAction(nameof(Edit), new { id = product.ProductID });
                }
            }
            else
            {
                // a new product
                productRepository.Add(product);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult New()
        {
            return EditProductInternal(new Product());
        }
    }
}