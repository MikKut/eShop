using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart.Middleware
{
    /// <summary>
    /// Custom middleware to log requests and responses
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);
            await _next(context);
            LogResponse(context);
        }

        private void LogRequest(HttpContext context)
        {
            _logger.LogInformation("Request from {User} at {Path}: {RequestMethod}", context.User?.Identity.Name, context.Request.Path, context.Request.Method);
        }

        private void LogResponse(HttpContext context)
        {
            _logger.LogInformation("Response for {User} at {Path}: {StatusCode}", context.User?.Identity.Name, context.Request.Path, context.Response.StatusCode);
        }
    }

}
