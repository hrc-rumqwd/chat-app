using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Commands
{
    public class AdminSetUserActivationCommand : ICommand<Result<bool>>
    {
        public long UserId { get; set; }
        public bool IsActived { get; set; }
    }

    public class AdminSetUserActivationCommandHandler(ApplicationDbContext context)
        : ICommandHandler<AdminSetUserActivationCommand, Result<bool>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<bool>> Handle(AdminSetUserActivationCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            if (user is null)
            {
                return Result<bool>.Failure("User not found");
            }

            user.IsActived = request.IsActived;
            _ = await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
