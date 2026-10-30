namespace Identity.Infrastructure.Authentication
{
    /// <summary>Authentication scheme names offered by the Identity module.</summary>
    public static class AuthenticationSchemes
    {
        /// <summary>
        /// MVC token cookie: Login issues a module JWT and bumps it into this cookie,
        /// whose expiry matches the token's. When the token expires the cookie
        /// challenge redirects the browser to /Account/Login for a fresh token.
        /// </summary>
        public const string TokenCookie = "Stocka.TokenCookie";

        /// <summary>Standard bearer scheme — reserved for API surfaces that read the raw JWT.</summary>
        public const string JwtBearer = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    }
}
