using System.Linq;
using System.Threading.Tasks;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Epam.AspNet.Module1.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext context;

        public CategoriesController(NorthwindContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Categories.ToList());
        }

        private byte[] GetBitmapFix(byte[] raw)
        {
            if (raw[0] == 'B' && raw[1] == 'M')
                return raw;
            else return raw.Skip(78).ToArray();
        }

        /// <summary>
        /// Returns image (bmp)
        /// </summary>
        /// <param name="id">category id</param>
        public IActionResult Image(int id)
        {
            Category category = context.Categories.Find(id);
            byte[] imageBytes = GetBitmapFix(category.Picture);
            return this.File(imageBytes, "image/bmp");
        }

        public IActionResult DownloadImage(int id)
        {
            Category category = context.Categories.Find(id);
            byte[] imageBytes = GetBitmapFix(category.Picture);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = category.CategoryName + ".bmp",

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            return this.File(imageBytes, "image/bmp");
        }

        public IActionResult Edit(int id)
        {
            Category category = context.Categories.Find(id);
            if (category == null)
                return NotFound(id);
            return View(category);
        }   
        
        [HttpPost]
        public IActionResult Edit(Category category, IFormFile picture)
        {
            Category oldCategory = context.Categories.Find(category.CategoryID);
            if (oldCategory == null)
                return NotFound(category.CategoryID);

            if (TryUpdateModelAsync(oldCategory).Result)
            {
                if(picture!=null)
                {
                    using var stream = picture.OpenReadStream();
                    oldCategory.Picture = new byte[picture.Length];
                    stream.Read(oldCategory.Picture, 0, (int)picture.Length);
                }
                context.SaveChanges();
                return RedirectToAction(nameof(Edit), new { id = category.CategoryID });
            }

            return RedirectToAction(nameof(Edit), new { id = category.CategoryID });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cat = context.Categories.Find(id);
                if (cat == null)
                {
                    return NotFound();
                }
                context.Remove(cat);
                await context.SaveChangesAsync();
                return RedirectToAction();
            }
            catch(DbUpdateException ex)
            {
                // ref constraint exception
                throw;
            }
        }
    }
}