using ChatApp.Application.Contracts.Brokers;
using ChatApp.Shared.Models.Commons;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Authentication.Commands
{
    public class LoginCommand : ICommand<LoginCommandResult>
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remeber Me")]
        public bool? Remember { get; set; }
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
    {
        public Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class LoginCommandResult : ResultBase
    {
    }
}
