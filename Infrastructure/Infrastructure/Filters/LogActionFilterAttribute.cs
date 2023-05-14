using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters
{
    public class LogActionFilterAttribute<T> : IActionFilter
        where T : ControllerBase
    {
        private readonly ILogger<T> _logger;
        public LogActionFilterAttribute(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result == null)
            {
                _logger.LogInformation($"Returned \"Not found\" from {context.ActionDescriptor.DisplayName} action method of {typeof(T)}");
                context.Result = new BadRequestResult();
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                _logger.LogError($"Model state in {context.ActionDescriptor.DisplayName} of {nameof(T)} is invalid");
                return;
            }

            _logger.LogInformation($"Returned \"Ok\" from {context.ActionDescriptor.DisplayName} action method of {typeof(T)}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Entered {context.ActionDescriptor.DisplayName} action method from {typeof(T)}");
        }
    }
}
