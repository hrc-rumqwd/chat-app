using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.Services;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Queries
{
    public record GetUsersConnectionsQuery(
        IEnumerable<long> UserIds)
        : IQuery<Result<Dictionary<long, string[]>>>;

    public class GetUsersConnectionsQueryHandler(IPresenceTracker tracker) : IQueryHandler<GetUsersConnectionsQuery, Result<Dictionary<long, string[]>>>
    {
        public async Task<Result<Dictionary<long, string[]>>> Handle(GetUsersConnectionsQuery request, CancellationToken cancellationToken)
        {
            Dictionary<long, string[]> connectionsList = [];
            foreach (long userId in request.UserIds)
            {
                string[] connections = await tracker.GetUserConnectionsAsync(userId);
                connectionsList.Add(userId, connections);
            }

            return Result<Dictionary<long, string[]>>.Success(connectionsList);
        }
    }
}
