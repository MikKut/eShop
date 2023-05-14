using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Infrastructure.Identity;

public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        string? targetScope = GetTargetScope(context);
        if (targetScope != null)
        {
            IEnumerable<string> scopes = context.User
                .FindAll(c => c.Type == "scope")
                .SelectMany(x => x.Value.Split(' '));
            if (scopes.Contains(targetScope))
            {
                context.Succeed(requirement);
            }
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private string? GetTargetScope(AuthorizationHandlerContext context)
    {
        HttpContext httpContext = (HttpContext)context.Resource!;

        Endpoint? routeEndpoint = httpContext.GetEndpoint();
        ControllerActionDescriptor? descriptor = routeEndpoint?.Metadata
            .OfType<ControllerActionDescriptor>()
            .SingleOrDefault();

        if (descriptor != null)
        {
            ScopeAttribute? scopeAttribute = (ScopeAttribute?)descriptor.MethodInfo.GetCustomAttribute(typeof(ScopeAttribute))
                                 ?? (ScopeAttribute?)descriptor.ControllerTypeInfo.GetCustomAttribute(typeof(ScopeAttribute));

            return scopeAttribute?.ScopeName;
        }

        return null;
    }
}