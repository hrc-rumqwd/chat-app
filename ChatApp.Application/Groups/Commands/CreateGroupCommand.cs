using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;

namespace ChatApp.Application.Groups.Commands
{
    public record CreateGroupCommand(
        string GroupName)
        : ICommand<Result<CreateChatGroupCommandResult>>;
    

    public class CreateChatGroupCommandHandler(
        ApplicationDbContext context    
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
