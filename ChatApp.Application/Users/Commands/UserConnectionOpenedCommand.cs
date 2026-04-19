using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.Services;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Users.Commands
{
    public record UserConnectionOpenedCommand(
        long UserId,
        string ConnectionId
    ) : ICommand<Result<UserConnectionOpenedCommandResult>>;

    public class UserConnectionOpenedCommandHandler(
        IPresenceTracker tracker
        ) : ICommandHandler<UserConnectionOpenedCommand,
        Result<UserConnectionOpenedCommandResult>>
    {

        public async Task<Result<UserConnectionOpenedCommandResult>> Handle(UserConnectionOpenedCommand request, CancellationToken cancellationToken)
        {
            bool isFirstConnection = await tracker.ConnectionOpenedAsync(request.UserId, request.ConnectionId);
            return Result<UserConnectionOpenedCommandResult>.Success(new(isFirstConnection));
        }
    }

    public record UserConnectionOpenedCommandResult(
        bool IsFirstConnection
    );
}
