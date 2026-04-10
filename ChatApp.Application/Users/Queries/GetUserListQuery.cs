using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Shared.Dtos;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Infrastructure.Presence;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Queries
{
    public class GetUserListQuery : PaginationBase, IQuery<Result<GetUserListQueryResult>>
    {
        public long? CurrentUserId { get; set; }
    }

    public class GetUserListQueryHandler(
        ApplicationDbContext dbContext,
        IPresenceTracker tracker) : IQueryHandler<GetUserListQuery, Result<GetUserListQueryResult>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IPresenceTracker _tracker = tracker;

        public async Task<Result<GetUserListQueryResult>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {

            List<UserListItemDto> pagedUsers = await _dbContext.Users
                .AsNoTracking()
                .Where(x => x.Id != request.CurrentUserId)
                .OrderByDescending(e => e.CreatedAt)
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .Select(x => new UserListItemDto
                {
                    Id = x.Id,
                    Alias = x.Alias,
                    Dob = x.Dob,
                    FullName = x.FullName,
                })
                .ToListAsync(cancellationToken);

            if (pagedUsers.Count == 0)
            {
                return Result<GetUserListQueryResult>.Success(new([]));
            }

            long[] activeUsers = await _tracker.GetOnlineUsersAsync();

            foreach (UserListItemDto user in pagedUsers)
            {
                user.IsOnline = activeUsers.Contains(user.Id);
            }

            return Result<GetUserListQueryResult>.Success(new(pagedUsers));
        }
    }

    public record GetUserListQueryResult(ICollection<UserListItemDto> Users);
}
