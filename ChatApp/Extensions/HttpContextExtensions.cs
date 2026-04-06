using ChatApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ChatApp.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<long> GetUserIdAsync(this HttpContext context, string? authenticationScheme = null)
        {
            var principals = context.User;
            if(!string.IsNullOrEmpty(authenticationScheme))
            {
                var authResult = await context.AuthenticateAsync(authenticationScheme);
                principals = authResult.Principal;
            }

            string idVal = principals.FindFirstValue(IdentityClaims.UserId)
                ?? throw new Exception("User ID claim not found or invalid.");
            
            return long.Parse(idVal);
        }
    }
}
