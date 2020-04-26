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
using Microsoft.OpenApi.Models;
using System.Reflection;

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
            services.AddLogging(b => b.AddProvider(new TrivialFileLoggingProvider("log.txt")));

            services.AddControllersWithViews(options=> 
            {
                options.Filters.Add<LoggingFilter>();
            }).AddRazorRuntimeCompilation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<NorthwindContext>(options =>
                options
                .UseSqlServer(Configuration.GetConnectionString("local"))
                .UseLoggerFactory(_myLoggerFactory));

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        }

        // configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Application started from '{0}'", env.ContentRootPath);
            var configurationValues = Configuration.AsEnumerable().Select(kvp => string.Format("{0} = {1};", kvp.Key, kvp.Value)).ToArray();
            logger.LogInformation("Configuration:\r\n"+string.Join("\r\n", configurationValues));

            // note: simple "/Error" won't work, please use the path with no defaults
            app.UseExceptionHandler("/Home/Error");

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

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
