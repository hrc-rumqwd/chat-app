using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;
namespace ChatApp.Application.Users.Queries
{
    public record GetUserConnectionsQuery(long UserId) : IQuery<Result<string[]>>;
    public class GetUserConnectionsQueryHandler : IQueryHandler<GetUserConnectionsQuery, Result<string[]>>
    {
        private readonly IPresenceTracker _tracker;
        public GetUserConnectionsQueryHandler(IPresenceTracker tracker)
        {
            _tracker = tracker;
        }
        public async Task<Result<string[]>> Handle(GetUserConnectionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _tracker.GetUserConnectionsAsync(request.UserId);
            return Result<string[]>.Success(result);
        }
    }
}
