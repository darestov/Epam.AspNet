using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Epam.AspNet.Module1.Middleware
{
    public class LoggingFilter : IActionFilter
    {
        private readonly bool flag;
        private readonly IConfiguration configuration;
        ILogger logger;

        public LoggingFilter(ILogger<LoggingFilter> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.flag = configuration.GetValue<bool>("AppSettings:ActionLogging");
            this.configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (flag)
            {
                string controller = "<unknown>", action = "<unknown>";
                if (context.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor ad)
                {
                    controller = ad.ControllerName;
                    action = ad.ActionName;
                }
                logger.LogInformation($"Controller '{controller}' is about to execute action '{action}'");
            }
        }  

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (flag)
            {
                string controller="<unknown>", action= "<unknown>";
                if(context.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor ad)
                {
                    controller = ad.ControllerName;
                    action = ad.ActionName;
                }
                logger.LogInformation($"Controller '{controller}' has executed action '{action}'");
            }
        }
    }
}
