using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Groups.Commands;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController(IBroker broker) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateGroup(string groupName)
        {
            Result<CreateChatGroupCommandResult> result = await broker.CommandAsync(new CreateGroupCommand(groupName));
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
