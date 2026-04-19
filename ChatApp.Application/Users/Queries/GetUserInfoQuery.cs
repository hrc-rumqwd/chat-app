using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Data.Entities;
using ChatApp.Shared.Models.Commons;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Queries
{
    public record GetUserInfoQuery(long UserId) : IQuery<Result<GetUserInfoQueryResult>>;

    public class GetUserInfoQueryHandler(
        IApplicationDbContext context
        ) : IQueryHandler<GetUserInfoQuery, Result<GetUserInfoQueryResult>>
    {
        public async Task<Result<GetUserInfoQueryResult>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            AppUser? user = await context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
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
