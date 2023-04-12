using Infrastructure.Extensions;
using Infrastructure.Filters;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Order.Host;
using Order.Host.Controllers;
using Order.Host.Models.Dtos;
using Order.Host.Services;
using Order.Host.Services.Interfaces;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
})
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
    //.AddNewtonsoftJson(options =>
    // options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShop- Order HTTP API",
        Version = "v1",
        Description = "The Order Service HTTP API"
    });

    var authority = configuration["Authorization:Authority"];
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            ClientCredentials = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    { "basket", "basket.basketbff" },
                    { "basket.basketbff", "basket.basketbff" },
                    { "mvc", "website" },
                }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.AddConfiguration();
builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddAuthorization(configuration);
builder.Services.AddTransient<IInternalHttpClientService, InternalHttpClientService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
builder.Services.AddTransient<IOrderService<CatalogItemDto>, OrderService>();
builder.Services.AddScoped<LogActionFilterAttribute<OrderBffController>>();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Order.API V1");
        setup.OAuthClientId("orderswaggerui");
        setup.OAuthAppName("Order Swagger UI");
    });

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers();
});

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}