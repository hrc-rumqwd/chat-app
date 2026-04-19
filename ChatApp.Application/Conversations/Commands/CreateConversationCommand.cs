using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Data.Entities;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Conversations.Commands
{
    public record CreateConversationCommand(
        long RequestUserId,
        long DestinationUserId,
        IEnumerable<long>? GroupUsers = null
        ) : ICommand<Result<ConversationDto>>;

    public class CreateConversationCommandHandler : ICommandHandler<CreateConversationCommand, Result<ConversationDto>>
    {
        private readonly IApplicationDbContext _context;

        public CreateConversationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ConversationDto>> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            // Check conversation is existing
            var existed = await _context.Conversations
                .AsNoTracking()
                .Include(c => c.Participants)
                .Select(x => new
                {
                    x.Id,
                    ParticipantIds = x.Participants.Select(c => c.UserId).ToList()
                })
                .Where(c =>
                    c.ParticipantIds.Contains(request.RequestUserId)
                    && c.ParticipantIds.Contains(request.DestinationUserId)
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (existed != null)
            {
                return Result<ConversationDto>.Success(new()
                {
                    Id = existed.Id
                });
            }

            // Searching for users data
            Dictionary<long, AppUser> users = await _context.Users
                .AsNoTracking()
                .Where(u =>
                    u.Id == request.RequestUserId
                    || u.Id == request.DestinationUserId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FullName = x.FullName
                })
                .ToDictionaryAsync(c => c.Id, v => v, cancellationToken);

            // Create conversation for both users
            Conversation conversation = new()
            {
                IsGroup = false,
            };

            UserConversation sourceUc = new()
            {
                UserId = request.RequestUserId,
                Conversation = conversation,
                UserDisplayName = users[request.RequestUserId]?.FullName
            };

            UserConversation destinationUc = new()
            {
                UserId = request.DestinationUserId,
                Conversation = conversation,
                UserDisplayName = users[request.DestinationUserId]?.FullName
            };

            _ = _context.Conversations.Add(conversation);
            _context.UserConversations.AddRange(sourceUc, destinationUc);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return Result<ConversationDto>.Success(new() { Id = conversation.Id });
        }
    }
}
