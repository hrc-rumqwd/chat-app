using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Queries;
using ChatApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Web.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IBroker _broker;

        public UserController(IBroker broker)
        {
            _broker = broker;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var result = await _broker.QueryAsync(new GetUserInfoQuery(
                long.Parse(
                    User.FindFirstValue(IdentityClaims.UserId))));

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("{userId:long}")]
        public async Task<IActionResult> GetUserInfo(long userId)
        {
            var result = await _broker.QueryAsync(new GetUserInfoQuery(userId));
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
