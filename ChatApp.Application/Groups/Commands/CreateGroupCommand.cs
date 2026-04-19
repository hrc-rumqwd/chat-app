using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Groups.Commands
{
    public record CreateGroupCommand(
        string GroupName)
        : ICommand<Result<CreateChatGroupCommandResult>>;


    public class CreateChatGroupCommandHandler(
        IApplicationDbContext context
    ) : ICommandHandler<CreateGroupCommand, Result<CreateChatGroupCommandResult>>
    {
        public Task<Result<CreateChatGroupCommandResult>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateChatGroupCommandResult
    {

    }
}
