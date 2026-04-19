using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Application.Contracts.Tokens;
using ChatApp.Data.Entities;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChatApp.Application.Authentication.Commands
{
    public class JwtLoginCommand : ICommand<Result<JwtLoginCommandResult>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class JwtLoginCommandHandler(
        IApplicationDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenService jwtHandler
        ) : ICommandHandler<JwtLoginCommand, Result<JwtLoginCommandResult>>
    {
        public async Task<Result<JwtLoginCommandResult>> Handle(JwtLoginCommand request, CancellationToken cancellationToken)
        {
            AppUser user = await userManager.FindByNameAsync(request.UserName);
            
            if (user is null)
            {
                return Result<JwtLoginCommandResult>.Failure("User not found");
            }

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signInResult.IsNotAllowed)
            {
                return Result<JwtLoginCommandResult>.Failure("User is not allowed to sign in");
            }
            else if (signInResult.IsLockedOut)
            {
                return Result<JwtLoginCommandResult>.Failure("User is locked out");
            }
            if (!signInResult.Succeeded)
            {
                return Result<JwtLoginCommandResult>.Failure("Password is incorrect");
            }

            // Every thing is ok, generate access token and refresh token for user
            var accessToken = jwtHandler.CreateToken(await GetUserClaimsAsync(user));

            return Result<JwtLoginCommandResult>.Success(new JwtLoginCommandResult
            {
                AccessToken = accessToken,
            });
        }

        private async Task<ICollection<Claim>> GetUserClaimsAsync(AppUser user)
        {
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>
            {
                new Claim(IdentityClaims.UserId, user.Id.ToString()),
                new Claim(IdentityClaims.FullName, user.FullName),
                new Claim(IdentityClaims.Dob, user.Dob.ToUniversalTime().ToString("dd-MM-yyyy")),
                new Claim(IdentityClaims.Roles, string.Join(",", userRoles))
            };

            return claims;
        }
    }

    public class JwtLoginCommandResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
