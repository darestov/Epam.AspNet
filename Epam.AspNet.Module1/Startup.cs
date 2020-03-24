using Epam.AspNet.Module1.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Epam.AspNet.Module1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("local")));
            services.AddLogging(b => b.AddProvider(new TrivialFileLoggingProvider("log.txt")));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
