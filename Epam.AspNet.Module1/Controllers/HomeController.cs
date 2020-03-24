using Epam.AspNet.Module1.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Epam.AspNet.Module1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DoNotClick()
        {
            throw new ApplicationException("Oops! Told you.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string exceptionMessage = exceptionHandlerPathFeature?.Error?.GetType().Name ?? "Unknown error";
            
            if (exceptionHandlerPathFeature?.Path != null)
            {
                exceptionMessage += " happened on " + exceptionHandlerPathFeature.Path;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier , Message=exceptionMessage});
        }
    }
}