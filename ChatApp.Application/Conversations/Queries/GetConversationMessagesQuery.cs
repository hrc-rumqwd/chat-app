using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Conversations.Queries
{
    public class GetConversationMessagesQuery : PaginationBase, IQuery<Result<ICollection<MessageDto>>>
    {
        public long ConversationId { get; set; }
    }

    public class GetConversationMessagesQueryHandler : IQueryHandler<GetConversationMessagesQuery, Result<ICollection<MessageDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetConversationMessagesQueryHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ICollection<MessageDto>>> Handle(GetConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var conversation = await _context.Conversations
                .AsNoTracking()
                .Where(m => m.Id == request.ConversationId)
                .Select(x => new Conversation
                {
                    Id = x.Id,
                    Messages = x.Messages,
                    Participants = x.Participants,
                })
                .FirstOrDefaultAsync();

            if(conversation is null)
                return Result<ICollection<MessageDto>>.Failure("Conversation not found");

            var messages = conversation.Messages
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    SendAt = m.CreatedAt,
                    SendBy = m.SenderId
                })
                .OrderByDescending(m => m.SendAt)
                .ToList();

            var participantsInfo = await _context.Users
                .AsNoTracking()
                .Where(x => conversation.Participants.Select(p => p.UserId).Contains(x.Id))
                .Select(x => new MessageAuthorDto
                {
                    Id = x.Id,
                    DisplayName = x.FullName
                })
                .ToListAsync();

            messages.ForEach(m => m.Author = participantsInfo.FirstOrDefault(p => p.Id == m.SendBy));

            return Result<ICollection<MessageDto>>.Success(messages);
        }
    }
}
