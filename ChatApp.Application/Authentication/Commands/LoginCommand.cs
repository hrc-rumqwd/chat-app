using ChatApp.Infrastructure.Brokers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Application.Authentication.Commands
{
    public class LoginCommand : ICommand<LoginCommandResult>
    {
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
    {
        public Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class LoginCommandResult
    {
    }
}
