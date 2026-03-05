using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Messages.Dtos;

namespace ChatApp.Application.Chat.Commands
{
    public class SendMessageCommand : ICommand<SendMessageCommandResult>
    {
    }

    public class SendMessageCommandHandler(
        ApplicationDbContext dbContext   
    ) : ICommandHandler<SendMessageCommand, SendMessageCommandResult>
    {
        public Task<SendMessageCommandResult> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class SendMessageCommandResult : PaginationResult<MessageDto>
    {
    }
}
