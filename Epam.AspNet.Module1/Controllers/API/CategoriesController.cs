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

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetCategoryImage(int id)
        {
            var category = context.Categories.Find(id);
            if (category?.Picture==null)
                return NotFound();

            return new FileContentResult(category.Picture, "image/bmp");
        }

        
        [HttpPut("{id}/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCategoryImage(int id, IFormFile file)
        {
            var category = context.Categories.Find(id);
            if (category.Picture==null)
                return NotFound();

            using (var stream = file.OpenReadStream())
            {
                System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(stream);
                category.Picture = binaryReader.ReadBytes((int)file.Length);
                await context.SaveChangesAsync();
                return NoContent();
            }
  
        }


    }
}