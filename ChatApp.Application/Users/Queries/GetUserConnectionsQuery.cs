using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.Services;
using ChatApp.Shared.Models.Commons;
namespace ChatApp.Application.Users.Queries
{
    public record GetUserConnectionsQuery(long UserId) : IQuery<Result<string[]>>;
    public class GetUserConnectionsQueryHandler(IPresenceTracker tracker) : IQueryHandler<GetUserConnectionsQuery, Result<string[]>>
    {
        public async Task<Result<string[]>> Handle(GetUserConnectionsQuery request, CancellationToken cancellationToken)
        {
            string[] result = await tracker.GetUserConnectionsAsync(request.UserId);
            return Result<string[]>.Success(result);
        }
    }
}
