using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Commands
{
    public record UserConnectionClosedCommand(
        long UserId,
        string ConnectionId
    ) : ICommand<Result<UserConnectionClosedCommandResult>>;

    public class UserConnectionClosedCommandHandler
        : ICommandHandler<UserConnectionClosedCommand, Result<UserConnectionClosedCommandResult>>
    {
        private readonly IPresenceTracker _tracker;

        public UserConnectionClosedCommandHandler(IPresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public async Task<Result<UserConnectionClosedCommandResult>> Handle(UserConnectionClosedCommand request, CancellationToken cancellationToken)
        {
            bool isLastConnection = await _tracker.ConnectionClosedAsync(request.UserId, request.ConnectionId);
            return Result<UserConnectionClosedCommandResult>.Success(new(isLastConnection));
        }
    }

    public record UserConnectionClosedCommandResult(bool IsLastConnection);
}
