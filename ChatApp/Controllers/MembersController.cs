using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Members.Dtos;
using ChatApp.Application.Members.Queries;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController(IBroker broker) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMemberConversationList([FromQuery] GetMemberListQuery query)
        {
            // Reset user Id to ensure that the user cannot manipulate it
            query.RequestUserId = long.Parse(HttpContext.User.FindFirstValue(IdentityClaims.UserId));
            Result<ICollection<MemberDto>> result = await broker.QueryAsync(query);
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
