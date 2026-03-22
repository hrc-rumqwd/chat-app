using ChatApp.Application.Contracts.Brokers;
using ChatApp.Data.Entities;
using ChatApp.Infrastructure.Caching;
using ChatApp.Infrastructure.Extensions;
using ChatApp.Infrastructure.Persistence.Contexts;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace ChatApp.Application.Users.Queries
{
    public class GetUserListQuery : PaginationBase, IQuery<Result<GetUserListQueryResult>>
    {
    }

    public class GetUserListQueryHandler : IQueryHandler<GetUserListQuery, Result<GetUserListQueryResult>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICacheService _cache;

        public GetUserListQueryHandler(ApplicationDbContext dbContext,
            ICacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<Result<GetUserListQueryResult>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = _cache.Get<List<AppUser>>(CacheKeys.UserList);

            // Calculate max size of current cache based on page size and index
            int skipRowIndex = (request.PageIndex - 1) * request.PageSize;
            if (users is not null
                && (users.Count - skipRowIndex) > 0)
            {
                users = users.ToPaginatedList(request.PageIndex, request.PageSize);
                return Result<GetUserListQueryResult>.Success(new GetUserListQueryResult(users));
            }

            List<AppUser> pagedUsers = await _dbContext.Users
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync(cancellationToken);

            if(pagedUsers.Count == 0)
            {
                return Result<GetUserListQueryResult>.Success(new(new List<AppUser>()));
            }

            if (users is null)
            {
                users = pagedUsers;
            }
            else
            {
                users.AddRange(pagedUsers);
            }

            return Result<GetUserListQueryResult>.Success(new(_cache.Add(CacheKeys.UserList, users)));
        }
    }

    public record GetUserListQueryResult(ICollection<AppUser> Users);


    #region Cache Keys
    public class CacheKeys
    {
        public static string UserList => "users.list";
    }
    #endregion
}
