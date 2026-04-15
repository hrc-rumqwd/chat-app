using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Queries
{
    public record GetUserInfoQuery(long UserId) : IQuery<Result<GetUserInfoQueryResult>>;

    public class GetUserInfoQueryHandler(
        ApplicationDbContext context
        ) : IQueryHandler<GetUserInfoQuery, Result<GetUserInfoQueryResult>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<GetUserInfoQueryResult>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            AppUser? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            return user is null
                ? Result<GetUserInfoQueryResult>.Failure("User not found")
                : Result<GetUserInfoQueryResult>.Success(new()
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    FullName = user.FullName,
                    Dob = user.Dob,
                    Alias = user.Alias,
                    IsActived = user.IsActived
                });
        }
    }

    public class GetUserInfoQueryResult
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string? Alias { get; set; }
        public bool IsActived { get; set; }
    }
}
