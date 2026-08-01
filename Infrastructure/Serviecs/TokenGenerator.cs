using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Security;
using Application.Dtos.Auth;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Serviecs
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JWT _jwt;

        public TokenGenerator(IOptions<JWT> options)
        {
            _jwt = options.Value;
        }

        public string GenerateJWTToken(UserTokenDetailsDto userDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userDetails.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimNames.UserId, userDetails.UserId.ToString()),
                new(ClaimNames.ActiveBrandId, userDetails.ActiveBrandId.ToString()),
                new(ClaimNames.BrandRole, userDetails.BrandRole.ToString()),
            };

            if (userDetails.ActiveBranchId.HasValue)
                claims.Add(new Claim(ClaimNames.ActiveBranchId, userDetails.ActiveBranchId.Value.ToString()));
            

            claims.AddRange(userDetails.Roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwt.TokenExpirationInMinutes)),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
