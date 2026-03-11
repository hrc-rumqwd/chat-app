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
        public bool? Remember { get; set; }
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
            var user = await userManager.FindByNameAsync(request.UserName);

            if(user is null)
            {
                Result<LoginCommandResult>.Failure("User is not exist");
            }

            SignInResult signInResult = await signInManager.PasswordSignInAsync(
                user, request.Password, request.Remember ?? false, false);

            if(signInResult.IsLockedOut)
                Result<LoginCommandResult>.Failure(nameof(signInResult.IsLockedOut));
            
            if(signInResult.IsNotAllowed)
                Result<LoginCommandResult>.Failure(nameof(signInResult.IsNotAllowed));
            
            if(signInResult.RequiresTwoFactor)
                Result<LoginCommandResult>.Failure(nameof(signInResult.RequiresTwoFactor));

            // If bypass all above conditions, then login
            await httpContextAccessor.HttpContext.SignInAsync(
                GetUserClaims(user));

            return Result<LoginCommandResult>.Success(new LoginCommandResult());
        }

        protected ClaimsPrincipal GetUserClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(IdentityClaims.UserId, user.Id.ToString()),
                new Claim(IdentityClaims.UserName, user.UserName),
                new Claim(IdentityClaims.FullName, user.FullName),
                new Claim(IdentityClaims.Dob, user.Dob.ToUniversalTime().ToString()),
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
    }

    public class LoginCommandResult
    {
    }
}
