using ChatApp.Application.Chat.Commands;
using ChatApp.Application.Contracts.Brokers;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    [ApiController]
    [Route("/chat")]
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
    }
}
