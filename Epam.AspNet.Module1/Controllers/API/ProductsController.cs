using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Epam.AspNet.Module1.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {  
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductsController(IConfiguration config, IUnitOfWork unitOfWork)
        {
            Config = config;
            this.unitOfWork = unitOfWork;
        }

        public IConfiguration Config { get; }


        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            return Ok(unitOfWork.Products.GetProduct(productId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts()
        {
            int max = Config.GetValue("AppSettings:MaxProducts", 0);
            return Ok(unitOfWork.Products.ListProductsWithCategoriesAndSuppliers(max));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            unitOfWork.Products.Add(product);
            unitOfWork.SaveChanges();
            return CreatedAtAction("GetProduct", new { productId = product.ProductID }, product);
        }
    }
}