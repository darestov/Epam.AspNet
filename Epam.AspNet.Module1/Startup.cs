using Epam.AspNet.Module1.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Epam.AspNet.Module1.Interfaces;
using Epam.AspNet.Module1.Middleware;
using System;
using System.IO;

namespace Epam.AspNet.Module1
{
    public class Startup
    {
        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
            new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            });

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<NorthwindContext>(options =>
                options
                .UseSqlServer(Configuration.GetConnectionString("local"))
                .UseLoggerFactory(_myLoggerFactory));
            services.AddLogging(b => b.AddProvider(new TrivialFileLoggingProvider("log.txt")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, NorthwindContext>();
        }

        // configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Application started from '{0}'", env.ContentRootPath);
            var configurationValues = Configuration.AsEnumerable().Select(kvp => string.Format("{0} = {1};", kvp.Key, kvp.Value)).ToArray();
            logger.LogInformation("Configuration:\r\n"+string.Join("\r\n", configurationValues));

            // note: simple "/Error" won't work, please use the path with no defaults
            app.UseExceptionHandler("/Home/Error");
            
            app.UseStaticFiles();
            app.UseRouting();

            // use our custom image caching
            app.UseImageCaching(new ImageCachingOptions
            {
                ControllerName = "Categories", ActionName = "DownloadImage",
                CacheCapacity = 3,
                CacheDirectoryPath = Path.Combine(Path.GetTempPath(), "Epam.AspNetCore.ImageCache"),
                ExpirationInterval = TimeSpan.FromMinutes(1)
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("images", "images/{id}", new { controller = "Categories", action = "DownloadImage" });
            });
        }
    }
}
