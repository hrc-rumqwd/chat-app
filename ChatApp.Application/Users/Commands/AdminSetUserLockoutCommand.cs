using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Application.Users.Commands
{
    public class AdminSetUserLockoutCommand : ICommand<Result<bool>>
    {
        public long UserId { get; set; }
        public bool IsLocked { get; set; }
    }

    public class AdminSetUserLockoutCommandHandler(UserManager<AppUser> userManager)
        : ICommandHandler<AdminSetUserLockoutCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AdminSetUserLockoutCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                return Result<bool>.Failure("User not found");
            }

            DateTimeOffset? lockoutEnd = request.IsLocked
                ? DateTimeOffset.UtcNow.AddYears(100)
                : null;

            IdentityResult result = await userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            if (!result.Succeeded)
            {
                return Result<bool>.Failure(result.Errors?.FirstOrDefault()?.Description);
            }

            return Result<bool>.Success(true);
        }
    }
}
