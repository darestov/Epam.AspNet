using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
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

        public ProductsController(IConfiguration config, IUnitOfWork unitOfWork, IProductRepository productRepository, ISupplierRepository supplierRepository, ICategoryRepository categoryRepository)
        {
            Config = config;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.supplierRepository = supplierRepository;
            this.categoryRepository = categoryRepository;
        }

        public IConfiguration Config { get; }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            int max = Config.GetValue("AppSettings:MaxProducts", 0);
            return Ok(productRepository.ListProductsWithCategoriesAndSuppliers(max));
        }
    }
}