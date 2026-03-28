using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Webworks.Service
{
    public class TokenService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;

        public TokenService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<(string Token, DateTime ExpiresAt)> GenerateToken(IdentityUser user)
        {
            if(string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
            {
                throw new ArgumentException("User must have a valid email and username.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:TokenKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var expirationDays = config.GetValue<int>("JWTSettings:ExpirationDays", 7);
            var expiresAt = DateTime.UtcNow.AddDays(expirationDays);

            var tokenOptions = new JwtSecurityToken
            (
                issuer: null,
                audience: null,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return (Token: token, ExpiresAt: expiresAt);
        }
    }
}
