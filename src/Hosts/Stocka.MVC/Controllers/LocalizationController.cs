using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Stocka.MVC.Controllers;

public sealed class LocalizationController : Controller
{
    [HttpGet]
    public IActionResult SetLanguage(string culture, string? returnUrl = null)
    {
        if (culture is not ("ar" or "en")) return BadRequest();

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });

        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
    }
}
