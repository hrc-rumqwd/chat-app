using ChatApp.Application.Authentication.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        [Route("/login")]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginCommand command)
        {
            return Ok();
        }

        [Route("/sign-up")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("/sign-up")]
        public async Task<IActionResult> SignUp(SignUpCommand command)
        {
            return Ok();
        }
    }
}
