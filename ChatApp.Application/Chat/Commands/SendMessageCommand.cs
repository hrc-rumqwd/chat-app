using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Messages.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Chat.Commands
{
    public record SendMessageCommand(
        long SenderId,
        string Content
    ) :
    ICommand<Result<SendMessageCommandResult>>;
    

    public class SendMessageCommandHandler(
        ApplicationDbContext dbContext   
    ) : ICommandHandler<SendMessageCommand, Result<SendMessageCommandResult>>
    {
        public async Task<Result<SendMessageCommandResult>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            // Check user is exist and active
            AppUser? user = await dbContext.Users
                .Where(x => x.Id == request.SenderId)
                .Select(x => new AppUser 
                {
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Id = x.Id,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if(user is null)
            {
                return Result<SendMessageCommandResult>.Failure("User not found");
            }

            Message m = new Message()
            {
                Content = request.Content,
                UserId = request.SenderId,
            };

            dbContext.Messages.Add(m);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<SendMessageCommandResult>.Success(new SendMessageCommandResult()
            {
                UserId = request.SenderId,
                Content = request.Content,
                SenderName = user.FullName,
                SentAt = m.CreatedAt
            });
        }
    }

    public class SendMessageCommandResult 
    {
        public long UserId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
