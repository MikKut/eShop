using Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        _ = builder.Services.Configure<ClientConfig>(
            builder.Configuration.GetSection("Client"));
        _ = builder.Services.Configure<AuthorizationConfig>(
            builder.Configuration.GetSection("Authorization"));
    }
}