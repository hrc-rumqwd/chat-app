using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Authentication.Commands
{
    public class SignUpCommand : ICommand<Result<SignUpCommandResult>>
    {
        [Required(ErrorMessage = "User Name must not be empty")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [Display(Name = "Repeat Password")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessage = "Full name must not be empty")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth must not be empty")]
        [Display(Name = "Date of birth")]
        public DateTime Dob { get; set; }
    }

    public class SignUpCommandHandler(
        UserManager<AppUser> userManager
        ) : ICommandHandler<SignUpCommand, Result<SignUpCommandResult>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<Result<SignUpCommandResult>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            AppUser? exist = await _userManager.FindByNameAsync(request.UserName);

            if (exist != null)
            {
                return Result<SignUpCommandResult>.Failure("User name already exists");
            }

            AppUser user = new()
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Dob = request.Dob.ToUniversalTime(),
                IsActived = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return Result<SignUpCommandResult>.Failure(result.Errors?.FirstOrDefault()?.Description);
            }

            IdentityResult addUserRoleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);
            if (!addUserRoleResult.Succeeded)
            {
                return Result<SignUpCommandResult>.Failure(addUserRoleResult.Errors?.FirstOrDefault()?.Description);
            }

            return Result<SignUpCommandResult>.Success(new SignUpCommandResult
            {
                UserId = user.Id,
                UserName = user.UserName
            });
        }
    }

    public class SignUpCommandResult
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
