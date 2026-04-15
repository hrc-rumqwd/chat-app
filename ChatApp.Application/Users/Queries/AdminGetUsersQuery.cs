using ChatApp.Application.Contracts.Brokers;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Users.Queries
{
    public class AdminGetUsersQuery : PaginationBase, IQuery<Result<AdminGetUsersQueryResult>>
    {
        public string? SearchKeyword { get; set; }
    }

    public class AdminGetUsersQueryHandler(ApplicationDbContext context)
        : IQueryHandler<AdminGetUsersQuery, Result<AdminGetUsersQueryResult>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AdminGetUsersQueryResult>> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Data.Entities.AppUser> query = _context.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                string keyword = request.SearchKeyword.Trim().ToLower();
                query = query.Where(x =>
                    x.UserName!.ToLower().Contains(keyword)
                    || x.FullName.ToLower().Contains(keyword)
                    || (x.Alias != null && x.Alias.ToLower().Contains(keyword)));
            }

            List<AdminUserListItemDto> users = await query
                .OrderByDescending(x => x.CreatedAt)
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .Select(x => new AdminUserListItemDto
                {
                    UserId = x.Id,
                    UserName = x.UserName!,
                    FullName = x.FullName,
                    Alias = x.Alias,
                    IsActived = x.IsActived,
                    IsLocked = x.LockoutEnd.HasValue && x.LockoutEnd > DateTimeOffset.UtcNow
                })
                .ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                return Result<AdminGetUsersQueryResult>.Success(new([]));
            }

            long[] userIds = users.Select(x => x.UserId).ToArray();
            Dictionary<long, string[]> roleMap = await _context.UserRoles
                .AsNoTracking()
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(_context.Roles.AsNoTracking(),
                    ur => ur.RoleId,
                    role => role.Id,
                    (ur, role) => new { ur.UserId, RoleName = role.Name! })
                .GroupBy(x => x.UserId)
                .ToDictionaryAsync(x => x.Key, x => x.Select(r => r.RoleName).ToArray(), cancellationToken);

            foreach (AdminUserListItemDto user in users)
            {
                user.Roles = roleMap.TryGetValue(user.UserId, out string[]? roles)
                    ? roles
                    : [];
            }

            return Result<AdminGetUsersQueryResult>.Success(new(users));
        }
    }

    public record AdminGetUsersQueryResult(ICollection<AdminUserListItemDto> Users);

    public class AdminUserListItemDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Alias { get; set; }
        public bool IsActived { get; set; }
        public bool IsLocked { get; set; }
        public string[] Roles { get; set; } = [];
    }
}
