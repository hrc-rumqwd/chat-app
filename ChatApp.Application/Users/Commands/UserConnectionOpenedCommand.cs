using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Commands
{
    public record UserConnectionOpenedCommand(
        long UserId,
        string ConnectionId
    ) : ICommand<Result<UserConnectionOpenedCommandResult>>;

    public class UserConnectionOpenedCommandHandler : ICommandHandler<UserConnectionOpenedCommand, 
        Result<UserConnectionOpenedCommandResult>>
    {
        private readonly IPresenceTracker _tracker;

        public UserConnectionOpenedCommandHandler(
            IPresenceTracker tracker
        )
        {
            _tracker = tracker;
        }

        public async Task<Result<UserConnectionOpenedCommandResult>> Handle(UserConnectionOpenedCommand request, CancellationToken cancellationToken)
        {
            bool isFirstConnection = await _tracker.ConnectionOpenedAsync(request.UserId, request.ConnectionId);
            return Result<UserConnectionOpenedCommandResult>.Success(new(isFirstConnection));
        }
    }

    public record UserConnectionOpenedCommandResult(
        bool IsFirstConnection
    );
}
