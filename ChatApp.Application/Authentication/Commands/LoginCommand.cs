using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChatApp.Application.Authentication.Commands
{
    public class LoginCommand : ICommand<Result<LoginCommandResult>>
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remeber Me")]
        public bool Remember { get; set; } = false;
        public string? ReturnUrl { get; set; }
    }

    public class LoginCommandHandler(
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        SignInManager<AppUser> signInManager
    ) : ICommandHandler<LoginCommand, Result<LoginCommandResult>>
    {
        public async Task<Result<LoginCommandResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Searching for the user by username
            AppUser? user = await userManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                return Result<LoginCommandResult>.Failure("User is not exist");
            }

            if (!user.IsActived)
            {
                return Result<LoginCommandResult>.Failure("User is inactive");
            }

            SignInResult signInResult = await signInManager.PasswordSignInAsync(
                user, request.Password, request.Remember, false);

            if (signInResult.IsLockedOut)
            {
                return Result<LoginCommandResult>.Failure(nameof(signInResult.IsLockedOut));
            }

            if (signInResult.IsNotAllowed)
            {
                return Result<LoginCommandResult>.Failure(nameof(signInResult.IsNotAllowed));
            }

            if (signInResult.RequiresTwoFactor)
            {
                return Result<LoginCommandResult>.Failure(nameof(signInResult.RequiresTwoFactor));
            }

            // If bypass all above conditions, then login
            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                await GetUserClaimsAsync(user),
                new AuthenticationProperties
                {
                    IsPersistent = request.Remember,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    AllowRefresh = true
                });

            return Result<LoginCommandResult>.Success(new LoginCommandResult(request.ReturnUrl));
        }

        protected async Task<ClaimsPrincipal> GetUserClaimsAsync(AppUser user)
        {
            List<Claim> claims =
            [
                new Claim(IdentityClaims.UserId, user.Id.ToString()),
                new Claim(IdentityClaims.UserName, user.UserName),
                new Claim(IdentityClaims.FullName, user.FullName),
                new Claim(IdentityClaims.Dob, user.Dob.ToUniversalTime().ToString()),
            ];

            IList<string> roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            ClaimsIdentity claimsIdentity = new(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            return claimsPrincipal;
        }
    }

    public record LoginCommandResult(
        string? ReturnUrl
    );
}
