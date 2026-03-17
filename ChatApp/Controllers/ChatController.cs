using ChatApp.Application.Chat.Commands;
using ChatApp.Application.Chat.Queries;
using ChatApp.Application.Contracts.Brokers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [Controller]
    public class ChatController(IBroker broker) : ControllerBase
    {
        public async Task<IActionResult> CreateGroup(CreateChatGroupCommand command)
        {
            var result = await broker.CommandAsync(command);
            return Ok(result);
        }

        public async Task<IActionResult> SendMessage(SendMessageCommand command)
        {
            var result = await broker.CommandAsync(command);
            return Ok(result);
        }

        [HttpGet("/api/chat/{groupId:long?}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMessages(long? groupId, [FromQuery]int pageIndex, [FromQuery] int pageSize)
        {
            var result = await broker.QueryAsync(new GetMessagesQuery(groupId, pageIndex, pageSize));
            return result.IsSuccess
                ? Ok(result) 
                : BadRequest(result);
        }
    }
}
