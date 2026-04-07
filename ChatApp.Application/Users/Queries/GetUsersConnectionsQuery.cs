using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Queries
{
    public record GetUsersConnectionsQuery(
        IEnumerable<long> UserIds)
        : IQuery<Result<Dictionary<long, string[]>>>;

    public class GetUsersConnectionsQueryHandler : IQueryHandler<GetUsersConnectionsQuery, Result<Dictionary<long, string[]>>>
    {
        private readonly IPresenceTracker _tracker;

        public GetUsersConnectionsQueryHandler(IPresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public async Task<Result<Dictionary<long, string[]>>> Handle(GetUsersConnectionsQuery request, CancellationToken cancellationToken)
        {
            Dictionary<long, string[]> connectionsList = new Dictionary<long, string[]>();
            foreach(var userId in request.UserIds)
            {
                var connections = await _tracker.GetUserConnectionsAsync(userId);
                connectionsList.Add(userId, connections);
            }

            return Result<Dictionary<long, string[]>>.Success(connectionsList);
        }
    }
}
