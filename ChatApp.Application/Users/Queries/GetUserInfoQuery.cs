using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Queries
{
    public record GetUserInfoQuery(long UserId) : IQuery<Result<GetUserInfoQueryResult>>;

    public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, Result<GetUserInfoQueryResult>>
    {
        private readonly ApplicationDbContext _context;

        public GetUserInfoQueryHandler(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public async Task<Result<GetUserInfoQueryResult>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            return user is null
                ? Result<GetUserInfoQueryResult>.Failure("User not found")
                : Result<GetUserInfoQueryResult>.Success(
                    user.Adapt<GetUserInfoQueryResult>());
        }
    }

    public class GetUserInfoQueryResult
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string Alias { get; set; }
    }
}
