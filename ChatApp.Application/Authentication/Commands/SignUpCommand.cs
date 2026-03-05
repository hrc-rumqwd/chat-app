using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Authentication.Commands
{
    public class SignUpCommand : ICommand<SignUpCommandResult>
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

    public class SignUpCommandHandler : ICommandHandler<SignUpCommand, SignUpCommandResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SignUpCommandHandler(
            ApplicationDbContext context,
            UserManager<AppUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<SignUpCommandResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            AppUser? exist = await _userManager.FindByNameAsync(request.UserName);

            if (exist != null)
            {
                return new SignUpCommandResult
                {
                    IsSucceed = false,
                    Message = "User name already exists"
                };
            }

            AppUser user = new AppUser
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Dob = request.Dob.ToUniversalTime()
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return ResultBase.Failed<SignUpCommandResult>(result.Errors?.FirstOrDefault()?.Description);
            }

            return new SignUpCommandResult
            {
                IsSucceed = true,
                Message = "Sign up successfully",
                Data = new SignUpCommandResultPayload
                {
                    UserId = user.Id,
                    UserName = user.UserName
                }
            };
        }
    }

    public class SignUpCommandResult : ResultBase<SignUpCommandResultPayload>
    {
    }

    public class SignUpCommandResultPayload
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
