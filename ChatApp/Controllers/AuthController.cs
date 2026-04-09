using ChatApp.Application.Authentication.Commands;
using ChatApp.Application.Contracts.Brokers;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [Controller]
    public class AuthController(IBroker broker) : Controller
    {
        private readonly IBroker _broker = broker;

        [Route("/login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            Result<LoginCommandResult> loginResult = await _broker.CommandAsync(command);

            return loginResult.IsSuccess
            ? Ok(loginResult)
            : BadRequest(loginResult);
        }

        [Route("/sign-up")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("/sign-up")]
        public async Task<IActionResult> SignUp(SignUpCommand command)
        {
            Result<SignUpCommandResult> result = await _broker.CommandAsync(command);
            return result.IsSuccess
                ? Created("/sign-up", result)
                : BadRequest(result);
        }

    }
}
