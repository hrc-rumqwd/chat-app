using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Queries
{
    public record GetUsersConnectionsQuery(
        IEnumerable<long> UserIds)
        : IQuery<Result<Dictionary<long, string[]>>>;

    public class GetUsersConnectionsQueryHandler(IPresenceTracker tracker) : IQueryHandler<GetUsersConnectionsQuery, Result<Dictionary<long, string[]>>>
    {
        private readonly IPresenceTracker _tracker = tracker;

        public async Task<Result<Dictionary<long, string[]>>> Handle(GetUsersConnectionsQuery request, CancellationToken cancellationToken)
        {
            Dictionary<long, string[]> connectionsList = [];
            foreach (long userId in request.UserIds)
            {
                string[] connections = await _tracker.GetUserConnectionsAsync(userId);
                connectionsList.Add(userId, connections);
            }

            return Result<Dictionary<long, string[]>>.Success(connectionsList);
        }
    }
}
