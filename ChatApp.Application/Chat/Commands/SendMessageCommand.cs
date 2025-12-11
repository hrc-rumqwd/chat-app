using ChatApp.Infrastructure.Brokers;
using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Messages.Dtos;

namespace ChatApp.Application.Chat.Commands
{
    public class SendMessageCommand : ICommand<SendMessageCommandResult>
    {
    }

    public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, SendMessageCommandResult>
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
