using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Users.Commands
{
    public class ChangeCurrentUserPasswordCommand : ICommand<Result<ChangeCurrentUserPasswordCommandResult>>
    {
        public long CurrentUserId { get; set; }

        [Required(ErrorMessage = "Current password must not be empty")]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password must not be empty")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password must not be empty")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match new password")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ChangeCurrentUserPasswordCommandHandler(UserManager<AppUser> userManager)
        : ICommandHandler<ChangeCurrentUserPasswordCommand, Result<ChangeCurrentUserPasswordCommandResult>>
    {
        public async Task<Result<ChangeCurrentUserPasswordCommandResult>> Handle(ChangeCurrentUserPasswordCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByIdAsync(request.CurrentUserId.ToString());
            if (user is null)
            {
                return Result<ChangeCurrentUserPasswordCommandResult>.Failure("User not found");
            }

            IdentityResult changePasswordResult = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return Result<ChangeCurrentUserPasswordCommandResult>.Failure(changePasswordResult.Errors?.FirstOrDefault()?.Description);
            }

            return Result<ChangeCurrentUserPasswordCommandResult>.Success(new()
            {
                UserId = user.Id
            });
        }
    }

    public class ChangeCurrentUserPasswordCommandResult
    {
        public long UserId { get; set; }
    }
}
