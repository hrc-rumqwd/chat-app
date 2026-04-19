using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Conversations.Queries
{
    public record GetConversationInformationByInvitationLinkQuery(string InvitationPath) : IQuery<Result<ConversationDto>>;

    public class GetConversationInformationByInvitationLinkQueryHandler : IQueryHandler<GetConversationInformationByInvitationLinkQuery, Result<ConversationDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetConversationInformationByInvitationLinkQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ConversationDto>> Handle(GetConversationInformationByInvitationLinkQuery request, CancellationToken cancellationToken)
        {
            var conversation = await _context.Conversations
                .Where(c => c.InvitationPath == request.InvitationPath)
                .Select(c => new ConversationDto
                {
                    Id = c.Id,
                    DisplayName = c.Title,
                    IsGroup = c.IsGroup,
                    ParticipantId = null, // This will be set when the user joins the conversation
                })
                .FirstOrDefaultAsync(cancellationToken);

            if(conversation == null)
            {
                return Result<ConversationDto>.Failure("Invalid invitation link.");
            }

            return Result<ConversationDto>.Success(conversation);
        }
    }
}
