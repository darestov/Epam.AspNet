using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Epam.AspNet.Module1.Models.DTO;
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

        /// <summary>
        /// Lists all categories (image bytes not included).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = from c in context.Categories
                        select new CategoryDto { 
                            CategoryID= c.CategoryID,
                            CategoryName = c.CategoryName,
                            Description = c.Description
                        };
            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Returns the image of the specified category.
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <response code="200">Imsge is found and returned</response>
        /// <response code="404">When the specified image is not found in the database</response>
        [HttpGet("{id}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryImage(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category?.Picture==null)
                return NotFound();

            return new FileContentResult(category.Picture, "image/bmp");
        }


        /// <summary>
        /// Changes image of the specified category.
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <param name="file">image file</param>
        /// <remarks>
        /// In Postman, select 'form-data' payload, then select 'file' type, and the file chooser will appear.
        /// </remarks>
        /// <response code="204">When successfully updates the image</response>
        /// <response code="404">When the specified image is not found in the database</response>
        /// <response code="415">When the datatype is not multipart/form-data</response>
        [HttpPut("{id}/image")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> UpdateCategoryImage(int id, IFormFile file)
        {
            var category = await context.Categories.FindAsync(id);
            if (category==null)
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