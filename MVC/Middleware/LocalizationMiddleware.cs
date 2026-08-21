namespace MVC.Middleware
{
    public class LocalizationMiddleware
    {
        private const string LangCookieName = "stocka.lang";

        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestedLang = context.Request.Query["lang"].ToString();

            if (requestedLang is "ar" or "en")
            {
                context.Response.Cookies.Append(
                    LangCookieName,
                    requestedLang,
                    new CookieOptions
                    {
                        Path = "/",
                        HttpOnly = true,
                        IsEssential = true,
                        MaxAge = TimeSpan.FromDays(365)
                    });

                // Rebuild the URL without the lang parameter, then redirect so the
                // cookie becomes the single source of truth for the language.
                var query = new QueryString();
                foreach (var key in context.Request.Query.Keys)
                {
                    if (!string.Equals(key, "lang", StringComparison.OrdinalIgnoreCase))
                        query = query.Add(key, context.Request.Query[key].ToString());
                }

                var target = context.Request.Path + query.ToUriComponent();
                if (string.IsNullOrEmpty(target))
                    target = "/";

                context.Response.Redirect(target, permanent: true);
                return;
            }

            await _next(context);
        }
    }
}