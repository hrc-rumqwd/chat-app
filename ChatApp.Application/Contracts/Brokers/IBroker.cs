using MediatR;
namespace ChatApp.Application.Contracts.Brokers
{
    public interface IBroker
    {
        Task CommandAsync(ICommand command);
        Task CommandAsync(ICommand command, CancellationToken cancellationToken);
        Task<TCommandResult> CommandAsync<TCommandResult>(ICommand<TCommandResult> command);
        Task<TCommandResult> CommandAsync<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken);
        Task QueryAsync(IQuery command);
        Task QueryAsync(IQuery command, CancellationToken cancellationToken);
        Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> command);
        Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> command, CancellationToken cancellationToken);
        Task PublishEventAsync(INotification notification);
    }

    public class Broker(IMediator mediator) : IBroker
    {
        public async Task CommandAsync(ICommand command)
        {
            await mediator.Send(command);
        }

        public async Task CommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
        }

        public async Task<TCommandResult> CommandAsync<TCommandResult>(ICommand<TCommandResult> command)
        {
            return await mediator.Send(command);
        }

        public async Task<TCommandResult> CommandAsync<TCommandResult>(ICommand<TCommandResult> command, CancellationToken cancellationToken)
        {
            return await mediator.Send(command, cancellationToken);
        }

        public async Task PublishEventAsync(INotification notification)
        {
            await mediator.Publish(notification);
        }

        public async Task QueryAsync(IQuery command)
        {
            await mediator.Send(command);
        }

        public async Task QueryAsync(IQuery command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
        }

        public async Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> command)
        {
            return await mediator.Send(command);
        }

        public async Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> command, CancellationToken cancellationToken)
        {
            return await mediator.Send(command, cancellationToken);
        }
    }
}
