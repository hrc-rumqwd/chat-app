using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Conversations.Commands
{
    public class CreateGroupConversationCommand : ICommand<Result<ConversationDto>>
    {
        public long HostUserId { get; set; }
        [Required]
        public IEnumerable<long> UserIds { get; set; }
    }

    public class CreateGroupConversationCommandHandler(ApplicationDbContext context) : ICommandHandler<CreateGroupConversationCommand, Result<ConversationDto>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<ConversationDto>> Handle(CreateGroupConversationCommand request, CancellationToken cancellationToken)
        {
            if (!request.UserIds?.Any() ?? false)
            {
                return Result<ConversationDto>.Failure("UserIds cannot be empty");
            }

            List<long> participants = request.UserIds.ToList();
            participants.Add(request.HostUserId);
            var users = await _context.Users
                .Where(u => participants.Contains(u.Id))
                .Select(c => new
                {
                    c.Id,
                    c.FullName,
                })
                .ToListAsync(cancellationToken);

            if (users.Count < request.UserIds.Count())
            {
                return Result<ConversationDto>.Failure("One or more userIds do not exist in application");
            }

            // After validating, process the command to create a group conversation
            DateTime now = DateTime.UtcNow;

            Conversation conversation = new Conversation
            {
                IsGroup = true,
                Title = string.Join(", ", users.Select(c => c.FullName)).Truncate(20)
            };

            conversation.Participants = [.. users
                .Select(u => new UserConversation
                {
                    UserId = u.Id,
                    UserDisplayName = u.FullName,
                    HiddenBefore = now,
                    Conversation = conversation
                })];

            _ = _context.Conversations.Add(conversation);

            _ = await _context.SaveChangesAsync();

            return Result<ConversationDto>.Success(new ConversationDto
            {
                Id = conversation.Id,
                DisplayName = conversation.Title,
                IsGroup = conversation.IsGroup,
                ParticipantId = null,
                IsOnline = false,
                LastMessage = null,
                LastMessageAt = null,
                UnreadCount = 0
            });
        }
    }
}
