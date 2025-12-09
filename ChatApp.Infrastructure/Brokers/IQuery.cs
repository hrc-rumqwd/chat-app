using MediatR;
namespace ChatApp.Infrastructure.Brokers
{
    public interface IQuery<TQueryResult> : IRequest<TQueryResult>
    {
    }

    public interface IQuery : IRequest
    {
    }

    public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult>
        where TQuery : IQuery<TQueryResult>
    {
    }

    public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery>
        where TQuery : IQuery
    {
    }
}
