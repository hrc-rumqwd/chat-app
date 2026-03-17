using System.Security.Claims;

namespace ChatApp.Shared.Constants
{
    public static class IdentityClaims
    {
        public const string UserId = "Uid";
        public const string UserName = ClaimTypes.Name;
        public const string Dob = "Dob";
        public const string FullName = "FullName";
    }
}
