using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Conversations.Commands;
using ChatApp.Application.Conversations.Dtos;
using ChatApp.Application.Conversations.Queries;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using ChatApp.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Web.Controllers
{
    [Controller]
    [Authorize]
    [Route("api/[Controller]")]
    public class ConversationsController(IBroker broker) : Controller
    {
        private readonly IBroker _broker = broker;

        [HttpGet("{conversationId:long}")]
        [Authorize]
        public async Task<IActionResult> GetConversationMessages(long conversationId, int pageIndex, int pageSize)
        {
            Result<ICollection<MessageDto>> result = await _broker.QueryAsync(new GetConversationMessagesQuery
            {
                ConversationId = conversationId,
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result.Error);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConversations(int pageIndex, int pageSize)
        {
            long userId = long.Parse(HttpContext.User.FindFirstValue(IdentityClaims.UserId));
            Result<ICollection<ConversationDto>> result = await _broker.QueryAsync(new GetConversationsQuery
            {
                UserId = userId,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result.Error);
        }

        [HttpPost("{userId:long}")]
        [Authorize]
        public async Task<IActionResult> CreateConversation(long userId)
        {
            long currentUserId = await HttpContext.GetUserIdAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Result<ConversationDto> result = await _broker.CommandAsync(new CreateConversationCommand(currentUserId, userId));
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result.Error);
        }

        [HttpPost("group")]
        [Authorize]
        public async Task<IActionResult> CreateGroupConversation(CreateGroupConversationCommand command)
        {
            long currentUserId = await HttpContext.GetUserIdAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            command.HostUserId = currentUserId;
            Result<ConversationDto> result = await _broker.CommandAsync(command);
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("{conversationId:long}/invitation-link")]
        [Authorize]
        public async Task<IActionResult> CreateInvitationLink([FromRoute]CreateInvitationLinkCommand command)
        {
            var result = await _broker.CommandAsync(command);
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        public async Task<IActionResult> GetConversationInformationByInvitationLink(string invitationPath)
        {
            var result = await _broker.QueryAsync(new GetConversationInformationByInvitationLinkQuery(invitationPath));
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("invite/{invitationPath:required}")]
        public async Task<IActionResult> JoinThroughInvitationLink(string invitationPath)
        {
            long userId = await HttpContext.GetUserIdAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var result = await _broker.CommandAsync(new JoinConversationThroughInvitationLinkCommand(userId, invitationPath));
            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
