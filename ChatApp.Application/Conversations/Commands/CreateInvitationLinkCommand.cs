using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Encoders;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application.Conversations.Commands
{
    public record CreateInvitationLinkCommand(
        [FromRoute(Name = "conversationId")]
        long ConversationId
    ) : ICommand<Result<CreateInvitationLinkCommandResult>>;

    public class CreateInvitationLinkCommandHandler : ICommandHandler<CreateInvitationLinkCommand, Result<CreateInvitationLinkCommandResult>>
    {
        private readonly IServiceProvider _sp;
        private readonly ApplicationDbContext _context;

        public CreateInvitationLinkCommandHandler(
            ApplicationDbContext context,
            IServiceProvider sp)
        {
            _sp = sp;
            _context = context;
        }

        public async Task<Result<CreateInvitationLinkCommandResult>> Handle(CreateInvitationLinkCommand request, CancellationToken cancellationToken)
        {
            IApplicationEncoder encoder = _sp.GetRequiredKeyedService<IApplicationEncoder>(Crc32Encoder.SERVICE_KEY);

            var conversation = await _context.Conversations
                .Where(c => c.Id == request.ConversationId)
                .Include(c => c.Participants)
                .Select(c => new Conversation
                {
                    Id = c.Id,
                    IsGroup = c.IsGroup,
                    Title = c.Title,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (conversation is null)
            {
                return Result<CreateInvitationLinkCommandResult>.Failure("Conversation not found.");
            }

            if (!conversation.IsGroup)
            {
                return Result<CreateInvitationLinkCommandResult>.Failure("Invitation link can only be created for group conversations.");
            }

            if (string.IsNullOrEmpty(conversation.InvitationPath))
            {
                string invitationPath = encoder.Encode($"{conversation.Id}_{conversation.Title}");
                conversation.InvitationPath = invitationPath;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<CreateInvitationLinkCommandResult>.Success(new CreateInvitationLinkCommandResult(conversation.InvitationPath));
        }
    }

    public record CreateInvitationLinkCommandResult(
        string InvitationLink
    );
}
