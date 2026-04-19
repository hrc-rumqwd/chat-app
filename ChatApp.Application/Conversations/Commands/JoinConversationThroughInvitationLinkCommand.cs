using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Data.Entities;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Conversations.Commands
{
    public record JoinConversationThroughInvitationLinkCommand(
        long UserId,
        string InvitationPath
    ) : ICommand<Result<JoinConversationThroughInvitationLinkCommandResult>>;

    public class JoinConversationThroughInvitationLinkCommandHandler : ICommandHandler<JoinConversationThroughInvitationLinkCommand, Result<JoinConversationThroughInvitationLinkCommandResult>>
    {
        private readonly IApplicationDbContext _context;

        public JoinConversationThroughInvitationLinkCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<JoinConversationThroughInvitationLinkCommandResult>> Handle(JoinConversationThroughInvitationLinkCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _context.Conversations
                .Where(c => c.InvitationPath == request.InvitationPath)
                .FirstOrDefaultAsync(cancellationToken);

            if (conversation == null)
            {
                return Result<JoinConversationThroughInvitationLinkCommandResult>.Failure("Invalid invitation link.");
            }

            if (!conversation.IsGroup)
            {
                return Result<JoinConversationThroughInvitationLinkCommandResult>.Failure("Cannot join a non-group conversation through an invitation link.");
            }

            AppUser? user = await _context.Users
                .Where(cp => cp.Id == request.UserId)
                .Select(c => new AppUser
                {
                    Id = c.Id,
                    FullName = c.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return Result<JoinConversationThroughInvitationLinkCommandResult>.Failure("User does not exist.");
            }

            bool isAlreadyParticipant = await _context.UserConversations
                .AnyAsync(cp => cp.ConversationId == conversation.Id && cp.UserId == request.UserId, cancellationToken);
            if (isAlreadyParticipant)
            {
                return Result<JoinConversationThroughInvitationLinkCommandResult>.Failure("User is already a participant of the conversation.");
            }

            var userConversation = new UserConversation
            {
                ConversationId = conversation.Id,
                UserId = request.UserId,
                HiddenBefore = DateTime.UtcNow,
                UserDisplayName = user.FullName
            };

            _context.UserConversations.Add(userConversation);
            await _context.SaveChangesAsync();

            return Result<JoinConversationThroughInvitationLinkCommandResult>.Success(new JoinConversationThroughInvitationLinkCommandResult(conversation.Id));
        }
    }

    public record JoinConversationThroughInvitationLinkCommandResult(
        long ConversationId
    );
}
