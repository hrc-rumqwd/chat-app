using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Persistence.Contexts;

namespace ChatApp.Application.Chat.Commands
{
    public class CreateChatGroupCommand : ICommand<CreateChatGroupCommandResult>
    {
    }

    public class CreateChatGroupCommandHandler(
        ApplicationDbContext context    
    ) : ICommandHandler<CreateChatGroupCommand, CreateChatGroupCommandResult>
    {
        public Task<CreateChatGroupCommandResult> Handle(CreateChatGroupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateChatGroupCommandResult
    {

    }
}
