using ChatApp.Application.Contracts.Brokers;
using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Messages.Dtos;

namespace ChatApp.Application.Chat.Queries
{
    public class GetAllMessageQuery : IQuery<GetAllMessageQueryResult>
    {
    }

    public class GetAllMessageQueryHandler : IQueryHandler<GetAllMessageQuery, GetAllMessageQueryResult>
    {
        public Task<GetAllMessageQueryResult> Handle(GetAllMessageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class GetAllMessageQueryResult : PaginationResult<MessageDto>
    {
    }
}
