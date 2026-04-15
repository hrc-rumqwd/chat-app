using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Users.Commands
{
    public class UpdateCurrentUserProfileCommand : ICommand<Result<UpdateCurrentUserProfileCommandResult>>
    {
        public long CurrentUserId { get; set; }

        [Required(ErrorMessage = "Full name must not be empty")]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Alias")]
        public string? Alias { get; set; }

        [Required(ErrorMessage = "Date of birth must not be empty")]
        [Display(Name = "Date of birth")]
        public DateTime Dob { get; set; }
    }

    public class UpdateCurrentUserProfileCommandHandler(ApplicationDbContext context)
        : ICommandHandler<UpdateCurrentUserProfileCommand, Result<UpdateCurrentUserProfileCommandResult>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<UpdateCurrentUserProfileCommandResult>> Handle(UpdateCurrentUserProfileCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.CurrentUserId, cancellationToken);
            if (user is null)
            {
                return Result<UpdateCurrentUserProfileCommandResult>.Failure("User not found");
            }

            user.FullName = request.FullName.Trim();
            user.Alias = string.IsNullOrWhiteSpace(request.Alias) ? null : request.Alias.Trim();
            user.Dob = request.Dob.ToUniversalTime();

            _ = await _context.SaveChangesAsync(cancellationToken);

            return Result<UpdateCurrentUserProfileCommandResult>.Success(new()
            {
                UserId = user.Id,
                FullName = user.FullName,
                Alias = user.Alias,
                Dob = user.Dob
            });
        }
    }

    public class UpdateCurrentUserProfileCommandResult
    {
        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public DateTime Dob { get; set; }
    }
}
