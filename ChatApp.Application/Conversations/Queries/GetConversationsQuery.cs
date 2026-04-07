using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Conversations.Queries
{
    public class GetConversationsQuery : PaginationBase, IQuery<Result<ICollection<ConversationDto>>>
    {
        public long UserId { get; set; }
    }

    public class GetConversationsQueryHandler : IQueryHandler<GetConversationsQuery, Result<ICollection<ConversationDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPresenceTracker _presenceTracker;

        public GetConversationsQueryHandler(
            ApplicationDbContext context,
            IPresenceTracker presenceTracker)
        {
            _context = context;
            _presenceTracker = presenceTracker;
        }

        public async Task<Result<ICollection<ConversationDto>>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _context.UserConversations
                .Where(x => x.UserId == request.UserId)
                .Include(x => x.Conversation)
                    .ThenInclude(c => c.Participants)
                        .ThenInclude(p => p.User)
                .Select(x => new ConversationDto
                {
                    Id = x.ConversationId,
                    IsGroup = x.Conversation.IsGroup,
                    DisplayName = x.Conversation.IsGroup
                        ? x.Conversation.Title
                        : x.Conversation.Participants
                            .Where(p => p.UserId != request.UserId)
                            .Select(x => x.User.FullName)
                            .FirstOrDefault() ?? "Unknown",
                    ParticipantId = x.Conversation.IsGroup
                        ? null
                        : x.Conversation.Participants
                            .Where(c => c.UserId != request.UserId)
                            .Select(c => c.UserId)
                            .FirstOrDefault(),
                    DisplayImage = null,
                    LastMessage = x.Conversation.LastMessageContent,
                    LastMessageAt = x.Conversation.LastMessageAt,
                })
                .OrderByDescending(c => c.LastMessageAt)
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .ToListAsync(cancellationToken);

            await HydrateOnlineStatusAsync(conversations);

            return Result<ICollection<ConversationDto>>.Success(conversations);
        }

        private async Task HydrateOnlineStatusAsync(ICollection<ConversationDto> conversations)
        {
            var oneOneOneConversations = conversations
                .Where(c => !c.IsGroup)
                .ToList();

            foreach (var conversation in oneOneOneConversations)
            {
                conversation.IsOnline = await _presenceTracker.IsUserOnline(conversation.ParticipantId.Value);
            }
        }
    }
}
