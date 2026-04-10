using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Chat.Queries
{
    public record GetMessagesQuery(
        long? GroupId,
        int PageIndex,
        int PageSize
    ) : IQuery<Result<ICollection<GetMessagesQueryResult>>>;


    public class GetMessagesQueryHandler(
        ApplicationDbContext context
    ) : IQueryHandler<GetMessagesQuery, Result<ICollection<GetMessagesQueryResult>>>
    {
        public async Task<Result<ICollection<GetMessagesQueryResult>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            // TODO: For group chat later
            //if(!request.GroupId.HasValue)
            //{
            //    return Result<ICollection<GetMessagesQueryResult>>.Failure("Group not found");
            //}

            List<GetMessagesQueryResult> messages = await context.Messages
                .OrderByDescending(x => x.CreatedAt)
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .Select(x => new GetMessagesQueryResult
                {
                    // Map properties from Message to GetMessagesQueryResult
                    Content = x.Content,
                    SenderId = x.SenderId,
                    SentAt = x.CreatedAt
                })
                .ToListAsync();

            var senders = await context.Users
                .Where(x => messages.Select(m => m.SenderId).Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            messages.ForEach(m =>
            {
                var sender = senders.FirstOrDefault(s => s.Id == m.SenderId);
                m.SenderName = sender != null ? sender.FullName : "Unknown";
                m.SentAt = m.SentAt.ToUniversalTime();
            });

            return Result<ICollection<GetMessagesQueryResult>>.Success(messages);
        }
    }

    public class GetMessagesQueryResult
    {
        public string Content { get; set; }
        public string SenderName { get; set; }
        public long SenderId { get; set; }
        public DateTime SentAt { get; set; }
    }
}
