using Epam.AspNet.Module1.Controllers;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace Epam.AspNet.Education.Tests
{
    public class CategoriesControllerTests
    {
        public const string DatabaseId = "Epam.AspNet.Education.InMemoryDb.Categories";
        public static DbContextOptions<NorthwindContext> Options { get; private set; }

        public CategoriesControllerTests()
        {
            Options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(DatabaseId)
                .Options;

            using (var db = new NorthwindContext(DatabaseHelper.Options))
            {
                db.Categories.Add(new Category { CategoryID = 1, CategoryName = "Cat1", Description = "" });
                db.Categories.Add(new Category { CategoryID = 2, CategoryName = "Cat2", Description = "" });
                db.SaveChanges();
            }
        }

        [Fact]
        public void CategoriesController_CreatesViewThatListsCategories()
        {
            // arrange
            using var db = new NorthwindContext(DatabaseHelper.Options);
            var controller = new CategoriesController(db);

            // act 
            IActionResult actionResult = controller.Index();

            // assert
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult.Model);
            Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.Model);
        }
    }
}
