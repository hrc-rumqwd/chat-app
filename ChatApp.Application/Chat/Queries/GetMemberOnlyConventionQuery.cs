using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Chat.Queries
{
    public record GetMemberOnlyConventionQuery(
        long sourceUserId,
        long destinationUserId)
        : IQuery<Result<GetMemberOnlyConventionQueryResult>>;

    public class GetMemberOnlyConventionQueryHandler(
        IApplicationDbContext context
        ) : IQueryHandler<GetMemberOnlyConventionQuery, Result<GetMemberOnlyConventionQueryResult>>
    {
        public Task<Result<GetMemberOnlyConventionQueryResult>> Handle(GetMemberOnlyConventionQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<GetMemberOnlyConventionQueryResult>.Success(new GetMemberOnlyConventionQueryResult()));
        }
    }

    public class GetMemberOnlyConventionQueryResult
    {

    }
}
