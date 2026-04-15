using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Commands;
using ChatApp.Application.Users.Queries;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = AppRoles.Admin)]
    [Route("api/admin/users")]
    public class AdminUsersController(IBroker broker) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] AdminGetUsersQuery query)
        {
            query.PageIndex = query.PageIndex <= 0 ? 1 : query.PageIndex;
            query.PageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            Result<AdminGetUsersQueryResult> users = await broker.QueryAsync(query);
            return users.IsSuccess
                ? Ok(users)
                : BadRequest(users);
        }

        [HttpPost("{userId:long}/activation")]
        public async Task<IActionResult> SetActivation(long userId, [FromBody] SetUserActivationRequest request)
        {
            Result<bool> result = await broker.CommandAsync(new AdminSetUserActivationCommand
            {
                UserId = userId,
                IsActived = request.IsActived
            });

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPost("{userId:long}/lockout")]
        public async Task<IActionResult> SetLockout(long userId, [FromBody] SetUserLockoutRequest request)
        {
            Result<bool> result = await broker.CommandAsync(new AdminSetUserLockoutCommand
            {
                UserId = userId,
                IsLocked = request.IsLocked
            });

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }

    public class SetUserActivationRequest
    {
        public bool IsActived { get; set; }
    }

    public class SetUserLockoutRequest
    {
        public bool IsLocked { get; set; }
    }
}
