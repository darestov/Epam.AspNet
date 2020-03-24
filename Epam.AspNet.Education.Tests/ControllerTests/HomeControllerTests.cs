using Epam.AspNet.Module1.Controllers;
using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace Epam.AspNet.Education.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void DoNotClickAction_ThrowsException()
        {
            var home = new HomeController();

            Assert.Throws<ApplicationException>(() => home.DoNotClick());
        }

        [Fact]
        public void Error_SetsRequestId()
        {
            // arrange
            var home = new HomeController();
            home.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            // act
            var actionResult = home.Error();

            // assert
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.NotNull(viewResult.Model);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
        }
    }
}
