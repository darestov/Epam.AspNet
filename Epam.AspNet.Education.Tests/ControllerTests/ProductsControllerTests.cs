using Epam.AspNet.Module1.Controllers;
using Epam.AspNet.Module1.DataAccess;
using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Epam.AspNet.Education.Tests
{
    public class ProductsControllerTests
    {
        const int MaxProducts = 4;
        Mock<IConfiguration> mockConfiguration;

        public ProductsControllerTests()
        {
            mockConfiguration = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns(MaxProducts.ToString());
            mockConfiguration.Setup(a => a.GetSection("AppSettings:MaxProducts")).Returns(configurationSection.Object);
        }

        [Fact]
        public void Controller_ReadsMaxPropertyAndReturnsListOfProducts()
        {
            Mock<ICategoryRepository> mockCategories = new Mock<ICategoryRepository>();
            Mock<ISupplierRepository> mockSuppliers = new Mock<ISupplierRepository>();
            Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            mockProducts.Setup(x => x.ListProductsWithCategoriesAndSuppliers(It.IsAny<int>())).Returns(new[] { new Product() });

            Mock<IUnitOfWork> mockUoW = new Mock<IUnitOfWork>();

            var controller = new ProductsController(mockConfiguration.Object, mockUoW.Object, mockProducts.Object, mockSuppliers.Object, mockCategories.Object);

            var actionResult = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult.Model);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            mockConfiguration.Verify(x=>x.GetSection("AppSettings:MaxProducts"));
        }

        [Fact]
        public void EditAction_ReturnsNotFound_WhenIdDoesNotExist()
        {
            Mock<ICategoryRepository> mockCategories = new Mock<ICategoryRepository>();
            Mock<ISupplierRepository> mockSuppliers = new Mock<ISupplierRepository>();
            Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            mockProducts.Setup(x => x.GetProduct(999)).Returns((Product)null);
            Mock<IUnitOfWork> mockUoW = new Mock<IUnitOfWork>();

            var controller = new ProductsController(mockConfiguration.Object, mockUoW.Object, mockProducts.Object, mockSuppliers.Object, mockCategories.Object);

            var actionResult = controller.Edit(999);

            Assert.IsType<NotFoundResult>(actionResult);

        }

        [Fact]
        public void PostEditAction_ValidatesError_WhenPriceIsGreaterThan1000()
        {
            Mock<ICategoryRepository> mockCategories = new Mock<ICategoryRepository>();
            Mock<ISupplierRepository> mockSuppliers = new Mock<ISupplierRepository>();
            Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            Mock<IUnitOfWork> mockUoW = new Mock<IUnitOfWork>();

            var controller = new ProductsController(mockConfiguration.Object, mockUoW.Object, mockProducts.Object, mockSuppliers.Object, mockCategories.Object);
            var product = new Product { UnitPrice = 2000 };

            controller.Edit(product);

            Assert.True(controller.ModelState.ContainsKey(nameof(Product.UnitPrice)));
        }

        [Fact]
        public void NewAction_ProvidesEmptyModelAndListsInTheViewBag()
        {
            Mock<ICategoryRepository> mockCategories = new Mock<ICategoryRepository>();
            Mock<ISupplierRepository> mockSuppliers = new Mock<ISupplierRepository>();
            Mock<IProductRepository> mockProducts = new Mock<IProductRepository>();
            Mock<IUnitOfWork> mockUoW = new Mock<IUnitOfWork>();

            mockCategories.Setup(x => x.ListCategories()).Returns(new[] { new Category(), new Category() });
            mockSuppliers.Setup(x => x.ListSuppliers()).Returns(new[] { new Supplier(), new Supplier() });

            var controller = new ProductsController(mockConfiguration.Object, mockUoW.Object, mockProducts.Object, mockSuppliers.Object, mockCategories.Object);

            var actionResult = controller.New();

            var viewResult = Assert.IsType<ViewResult>(actionResult);
            var model = Assert.IsType<Product>(viewResult.Model);
            Assert.NotNull(model);

            Assert.True(viewResult.ViewData.ContainsKey("CategoryID"));
            Assert.True(viewResult.ViewData.ContainsKey("SupplierID"));

            Assert.True((viewResult.ViewData["CategoryID"] as IEnumerable<SelectListItem>).Count() == 2);
            Assert.True((viewResult.ViewData["SupplierID"] as IEnumerable<SelectListItem>).Count() == 2);
        }
    }
}
