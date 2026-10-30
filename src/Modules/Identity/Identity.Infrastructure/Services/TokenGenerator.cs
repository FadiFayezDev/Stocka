using Identity.Contracts.DTOs;
using Identity.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Services
{
    /// <summary>
    /// JWT issuer over the configured symmetric key. Claims carried:
    /// sub, jti, userId (+ the standard nameidentifier) and one Role claim per
    /// assigned role — no more active-branch/brand claims; tenancy is resolved
    /// per request, not baked into the token.
    /// </summary>
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public TokenGenerator(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string GenerateToken(UserTokenDetailsDto userDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userDetails.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimNames.UserId, userDetails.UserId.ToString()),
                new(ClaimTypes.NameIdentifier, userDetails.UserId.ToString()),
            };

            // One Role claim per assigned role — token consumers read a multivalued claim.
            claims.AddRange(userDetails.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.DurationInHours),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
