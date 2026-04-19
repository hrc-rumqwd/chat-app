using ChatApp.Application.Contracts.Tokens;
using ChatApp.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatApp.Infrastructure.Tokens
{
    public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
    {
        public string CreateRefreshToken(string userId)
        {
            throw new NotImplementedException();
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            var secretKey = configuration["JwtConfiguration:SecretKey"];
            var credentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["JwtConfiguration:Issuer"],
                audience: configuration["JwtConfiguration:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(configuration["JwtConfiguration:ExpireMinutes"].ToInt32()),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
