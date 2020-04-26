using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Epam.AspNet.Module1.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext context;

        public CategoriesController(NorthwindContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await context.Categories.ToListAsync());
        }
    }
}