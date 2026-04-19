using System.Security.Claims;

namespace ChatApp.Application.Contracts.Tokens
{
    public interface IJwtTokenService
    {
        string CreateToken(IEnumerable<Claim> claims);
        string CreateRefreshToken(string userId);
    }
}
