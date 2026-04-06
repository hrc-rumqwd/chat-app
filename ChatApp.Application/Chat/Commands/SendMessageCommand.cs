using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Chat.Commands
{
    public record SendMessageCommand(
        string Content,
        long SenderId,
        long ConversationId
    ) :
    ICommand<Result<SendMessageCommandResult>>;
    

    public class SendMessageCommandHandler(
        ApplicationDbContext dbContext   
    ) : ICommandHandler<SendMessageCommand, Result<SendMessageCommandResult>>
    {
        public async Task<Result<SendMessageCommandResult>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {

            var conversation = await dbContext.Conversations
                .Where(x => x.Id == request.ConversationId)
                .Include(c => c.Participants)
                .ThenInclude(c => c.User)
                .Select(c => new Conversation
                {
                    Id = c.Id,
                    Participants = c.Participants
                        .Select(p => new UserConversation
                        {
                            Id = p.Id,
                            UserId = p.UserId,
                            UserDisplayName = p.UserDisplayName
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (conversation is null)
                return Result<SendMessageCommandResult>.Failure("Not found conversation");

            if (!IsUserInConversation(conversation, request.SenderId))
                return Result<SendMessageCommandResult>.Failure("User action is invalid");


            // Check user is exist and active
            bool userExisted = await dbContext.Users
                .Where(x => x.Id == request.SenderId)
                .AnyAsync(cancellationToken);

            if(!userExisted)
                return Result<SendMessageCommandResult>.Failure("User not found");

            Message m = new Message()
            {
                Content = request.Content,
                SenderId = request.SenderId,
                ConversationId = request.ConversationId
            };

            dbContext.Messages.Add(m);

            conversation.LastMessageContent = request.Content;
            conversation.LastMessageAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<SendMessageCommandResult>.Success(new SendMessageCommandResult()
            {
                SenderId = request.SenderId,
                SenderName = conversation.Participants.FirstOrDefault(p => p.UserId == request.SenderId).UserDisplayName,
                ReceiverId = conversation.Participants.FirstOrDefault(p => p.UserId != request.SenderId)?.UserId ?? 0,
                ReceiverName = conversation.Participants.FirstOrDefault(p => p.UserId != request.SenderId).UserDisplayName,
                Content = request.Content,
                ConversationId = conversation.Id,
                SentAt = m.CreatedAt,
            });
        }

        private bool IsUserInConversation(Conversation conversation, long userId)
            => conversation.Participants?.Any(c => c.UserId == userId) ?? false;
    }

    public class SendMessageCommandResult 
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public long ConversationId { get; set; }
        public long ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
