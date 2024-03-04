using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace IdentityServer
{
    public static class Config
    {
        // Add a logger instance
        private static readonly ILogger _logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // Set the minimum logging level
            .WriteTo.Console() // Log to console
            .CreateLogger();
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            _logger.Information("Retrieving identity resources...");
            var resourses = new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

            _logger.Information("Identity resources retrieved successfully.");
            return resourses;
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            _logger.Information("Retrieving API resources...");
            var apis =  new ApiResource[]
            {
                new ApiResource("petshop.com")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("mvc")
                    },
                },
                new ApiResource("catalog")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("catalog.catalogitem"),
                        new Scope("catalog.catalogbrand"),
                        new Scope("catalog.catalogtype"),
                        new Scope("catalog.catalogbff")
                    },
                },
                new ApiResource("basket")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("basket.basketbff"),
                    },
                },
                new ApiResource("order")
                {
                   Scopes = new List<Scope>
                   {
                      new Scope("order.makeorder")
                   },
                },
                new ApiResource("testing")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("integrationtesting.test")
                    },
                },
            };

            _logger.Information("API resources retrieved successfully.");
            return apis;
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            _logger.Information("Retrieving clients...");
            var clients = new[]
            {
                new Client
                {
                    ClientId = "mvc_pkce",
                    ClientName = "MVC PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RedirectUris = { $"{configuration["MvcUrl"]}/signin-oidc"},
                    AllowedScopes = {"openid", "profile", "mvc", "catalog.catalogitem", "catalog.catalogbff"},
                    RequirePkce = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = "catalog",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes =
                    {
                        "order.makeorder", "mvc", "profile", "catalog.catalogbff"
                    },

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                },
                new Client
                {
                    ClientId = "catalogswaggerui",
                    ClientName = "Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["CatalogApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["CatalogApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "openid", "profile", "mvc", "catalog.catalogitem","catalog.catalogtype", "catalog.catalogbrand", "catalog.catalogbff"
                    }
                },
                new Client
                {
                    ClientId = "basketswaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "mvc"
                    }
                },
                new Client
                {
                    ClientId = "basket",
                    ClientName = "Basket.Host",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "basket.basketbff", 
                        "order.makeorder",
                        "catalog.catalogbff"
                    }
                },
                new Client
                {
                    ClientId = "order",
                    ClientName = "Order.Host",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{configuration["OrderApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["OrderApi"]}/swagger/" },

                    AllowedScopes =
                    {
                       "catalog.catalogitem",
                       "basket.basketbff",
                       "order.makeorder"
                    }
                },
                new Client
                {
                    ClientId = "orderswaggerui",
                    ClientName = "Order Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["OrderApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["OrderApi"]}/swagger/" },

                    AllowedScopes =
                    {
                       "catalog.catalogitem",
                       "mvc",
                       "basket.basketbff",
                       "catalog.catalogbff"
                    }
                },
                new Client
                {
                    ClientId = "testing",
                    ClientName = "testing",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,

                    AllowedScopes =
                    {
                        "order.makeorder",
                        "basket.basketbff",
                        "catalog.catalogitem",
                        "catalog.catalogbrand",
                        "catalog.catalogtype",
                        "mvc",
                        "openid", 
                        "profile",
                        "catalog.catalogbff"
                    },
                },
            };

            _logger.Information("Clients retrieved successfully.");
            return clients;
        }
    }
}