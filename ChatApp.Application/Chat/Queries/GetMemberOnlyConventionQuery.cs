using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Chat.Queries
{
    public record GetMemberOnlyConventionQuery(
        long sourceUserId,
        long destinationUserId)
        : IQuery<Result<GetMemberOnlyConventionQueryResult>>;

    public class GetMemberOnlyConventionQueryHandler : IQueryHandler<GetMemberOnlyConventionQuery, Result<GetMemberOnlyConventionQueryResult>>
    {
        public GetMemberOnlyConventionQueryHandler(
            ApplicationDbContext context
        )
        {
        }

        public Task<Result<GetMemberOnlyConventionQueryResult>> Handle(GetMemberOnlyConventionQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<GetMemberOnlyConventionQueryResult>.Success(new GetMemberOnlyConventionQueryResult()));
        }
    }

    public class GetMemberOnlyConventionQueryResult
    {

    }
}
