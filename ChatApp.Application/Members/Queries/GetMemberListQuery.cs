using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Contracts.DbContext;
using ChatApp.Application.Contracts.Services;
using ChatApp.Application.Members.Dtos;
using ChatApp.Shared.Extensions;
using ChatApp.Shared.Models.Commons;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Members.Queries
{
    public class GetMemberListQuery : PaginationBase, IQuery<Result<ICollection<MemberDto>>>
    {
        public string? Search { get; set; }
        public long RequestUserId { get; set; }
    }

    public class GetMemberListQueryHandler : IQueryHandler<GetMemberListQuery, Result<ICollection<MemberDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPresenceTracker _presenceTracker;

        public GetMemberListQueryHandler(
            IApplicationDbContext context,
            IPresenceTracker presenceTracker)
        {
            _context = context;
            _presenceTracker = presenceTracker;
        }

        public async Task<Result<ICollection<MemberDto>>> Handle(GetMemberListQuery request, CancellationToken cancellationToken)
        {
            List<MemberDto> members = await _context.Users
                .AsNoTracking()
                .Where(c =>
                    c.Id != request.RequestUserId
                    && (string.IsNullOrEmpty(request.Search)
                        || EF.Functions.Like(c.FullName, $"{request.Search}%")
                ))
                .OrderByDescending(x => x.CreatedAt)
                .PaginatedQuery(request.PageIndex, request.PageSize)
                .Select(x => new MemberDto
                {
                    Id = x.Id,
                    DisplayName = x.FullName,
                    Avatar = string.Empty
                })
                .ToListAsync(cancellationToken);

            return Result<ICollection<MemberDto>>.Success(members);
        }
    }
}
