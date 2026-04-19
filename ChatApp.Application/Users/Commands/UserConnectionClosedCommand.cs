using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.Services;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Commands
{
    public record UserConnectionClosedCommand(
        long UserId,
        string ConnectionId
    ) : ICommand<Result<UserConnectionClosedCommandResult>>;

    public class UserConnectionClosedCommandHandler(IPresenceTracker tracker)
                : ICommandHandler<UserConnectionClosedCommand, Result<UserConnectionClosedCommandResult>>
    {
        public async Task<Result<UserConnectionClosedCommandResult>> Handle(UserConnectionClosedCommand request, CancellationToken cancellationToken)
        {
            bool isLastConnection = await tracker.ConnectionClosedAsync(request.UserId, request.ConnectionId);
            return Result<UserConnectionClosedCommandResult>.Success(new(isLastConnection));
        }
    }

    public record UserConnectionClosedCommandResult(bool IsLastConnection);
}
