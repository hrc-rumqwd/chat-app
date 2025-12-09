using MediatR;

namespace ChatApp.Infrastructure.Brokers
{
    public interface ICommand<TCommandResult> : IRequest<TCommandResult>
    {
    }

    public interface ICommand : IRequest
    {
    }

    public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>
        where TCommand : ICommand<TCommandResult>
    {
    }    

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }    
}
