using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        string authority = configuration["Authorization:Authority"];
        string siteAudience = configuration["Authorization:SiteAudience"];

        _ = services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
        _ = services
            .AddAuthentication()
            .AddJwtBearer(AuthScheme.Internal, options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            })
            .AddJwtBearer(AuthScheme.Site, options =>
            {
                options.Authority = authority;
                options.Audience = siteAudience;
                options.RequireHttpsMetadata = false;
            });
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.AllowEndUserPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(AuthScheme.Site);
                _ = policy.RequireClaim(JwtRegisteredClaimNames.Sub);
            });
            options.AddPolicy(AuthPolicy.AllowClientPolicy, policy =>
            {
                policy.AuthenticationSchemes.Add(AuthScheme.Internal);
                policy.Requirements.Add(new DenyAnonymousAuthorizationRequirement());
                policy.Requirements.Add(new ScopeRequirement());
            });
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
}