using System.Security.Claims;

namespace ChatApp.Shared.Constants
{
    public static class IdentityClaims
    {
        public static string UserId = "Uid";
        public static string UserName = ClaimTypes.Name;
        public static string Dob = "Dob";
        public static string FullName = "FullName";
    }
}
