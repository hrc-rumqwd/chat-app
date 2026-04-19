using ChatApp.Application.Chat.Dtos;
using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Data.Entities;
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
        IApplicationDbContext dbContext
    ) : ICommandHandler<SendMessageCommand, Result<SendMessageCommandResult>>
    {
        public async Task<Result<SendMessageCommandResult>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {

            Conversation? conversation = await dbContext.Conversations
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
            {
                return Result<SendMessageCommandResult>.Failure("Not found conversation");
            }

            if (!IsUserInConversation(conversation, request.SenderId))
            {
                return Result<SendMessageCommandResult>.Failure("User action is invalid");
            }


            // Check user is exist and active
            bool userExisted = await dbContext.Users
                .Where(x => x.Id == request.SenderId)
                .AnyAsync(cancellationToken);

            if (!userExisted)
            {
                return Result<SendMessageCommandResult>.Failure("User not found");
            }

            Message m = new()
            {
                Content = request.Content,
                SenderId = request.SenderId,
                ConversationId = request.ConversationId
            };

            _ = dbContext.Messages.Add(m);

            conversation.LastMessageContent = request.Content;
            conversation.LastMessageAt = DateTime.UtcNow;

            _ = await dbContext.SaveChangesAsync(cancellationToken);

            SendMessageCommandResult result = new SendMessageCommandResult()
            {
                SenderId = request.SenderId,
                SenderName = conversation.Participants.FirstOrDefault(p => p.UserId == request.SenderId).UserDisplayName,
                IsGroup = conversation.IsGroup,
                Content = request.Content,
                ConversationId = conversation.Id,
                SentAt = m.CreatedAt,
            };

            if (!conversation.IsGroup)
            {
                UserConversation? receiver = conversation.Participants.FirstOrDefault(p => p.UserId != request.SenderId);
                result.OneToOneReceiver = new ParticipantDto
                {
                    Id = receiver.Id,
                    DisplayName = receiver.UserDisplayName,
                };
            }
            else
            {
                result.Participants = [];
                conversation.Participants
                    .Where(p => p.UserId != request.SenderId)
                    .ToList()
                    .ForEach(r =>
                    {
                        result.Participants.Add(new ParticipantDto
                        {
                            Id = r.Id,
                            DisplayName = r.UserDisplayName,
                        });
                    });
            }

            return Result<SendMessageCommandResult>.Success(result);
        }

        private bool IsUserInConversation(Conversation conversation, long userId)
        {
            return conversation.Participants?.Any(c => c.UserId == userId) ?? false;
        }
    }

    public class SendMessageCommandResult
    {
        public long SenderId { get; set; }
        public string SenderName { get; set; }
        public long ConversationId { get; set; }
        public bool IsGroup { get; set; }
        public ParticipantDto OneToOneReceiver { get; set; }
        public ICollection<ParticipantDto> Participants { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
