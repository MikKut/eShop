using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("petShop.com")
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
                        new Scope("catalog.catalogtype")
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
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "mvc_pkce",
                    ClientName = "MVC PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RedirectUris = { $"{configuration["MvcUrl"]}/signin-oidc"},
                    AllowedScopes = {"openid", "profile", "mvc", "catalog.catalogitem"},
                    RequirePkce = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = "catalog",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes =
                    {
                        "order.makeorder"
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
                        "openid", "profile", "mvc", "catalog.catalogitem","catalog.catalogtype", "catalog.catalogbrand"
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
                        "order.makeorder"
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
                       "basket.basketbff"
                    }
                },
            };
        }
    }
}