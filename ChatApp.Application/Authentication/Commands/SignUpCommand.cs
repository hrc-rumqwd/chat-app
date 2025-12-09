using ChatApp.Infrastructure.Brokers;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.Authentication.Commands
{
    public class SignUpCommand : ICommand<SignUpCommandResult>
    {
        [Required(ErrorMessage = "User Name must not be empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full name must not be empty")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth must not be empty")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Alias must not be empty")]
        public string Alias { get; set; }
    }

    public class SignUpCommandHandler : ICommandHandler<SignUpCommand, SignUpCommandResult>
    {
        public Task<SignUpCommandResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class SignUpCommandResult
    {
    }
}
