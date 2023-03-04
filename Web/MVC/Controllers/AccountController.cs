using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MVC.Services;
using MVC.Services.Interfaces;
using MVC.ViewModels.Models;
using System.Net;

namespace MVC.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IIdentityParser<ApplicationUser> _identityParser;

    public AccountController(
        ILogger<AccountController> logger,
        IIdentityParser<ApplicationUser> identityParser)
    {
        _logger = logger;
        _identityParser = identityParser;
    }

    [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
    public IActionResult SignIn()
    {
        var user = _identityParser.Parse(User);

        _logger.LogInformation($"User #{user.Id} {user.Name} authenticated");
        
        // "Catalog" because UrlHelper doesn't support nameof() for controllers
        // https://github.com/aspnet/Mvc/issues/5853
        return RedirectToAction(nameof(CatalogController.Index), "Catalog");
    }

    [ProducesResponseType(typeof(Task<IActionResult>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        var user = _identityParser.Parse(User);

        _logger.LogInformation($"User #{user.Id} {user.Name} sighned out");
        // "Catalog" because UrlHelper doesn't support nameof() for controllers
        // https://github.com/aspnet/Mvc/issues/5853
        var homeUrl = Url.Action(nameof(CatalogController.Index), "Catalog");
        return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = homeUrl });
    }
}
