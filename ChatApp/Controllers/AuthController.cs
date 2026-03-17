using ChatApp.Application.Authentication.Commands;
using ChatApp.Application.Contracts.Brokers;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private readonly IBroker _broker;

        public AuthController(IBroker broker)
        {
            _broker = broker;
        }

        [Route("/login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var loginResult = await _broker.CommandAsync(command);
            
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
            var result = await _broker.CommandAsync(command);
            return result.IsSuccess
                ? Created("/sign-up", result)    
                : BadRequest(result);
        }

    }
}
