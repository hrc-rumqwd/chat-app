using ChatApp.Application.Chat.Commands;
using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Commands;
using ChatApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Web.Hubs
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ChatHub(
        IBroker broker
    ) : Hub
    {
        public async Task SendMessage(string message, long userId)
        {
            var sendMessageResult = await broker.CommandAsync(new SendMessageCommand(userId, message));
            
            if(sendMessageResult.IsSuccess != true)
            {
                await Clients.Caller.SendAsync("OnSendMessageError", sendMessageResult);
                return;
            }

            await Clients.All.SendAsync("ReceiveMessage", sendMessageResult);
        }

        // For update user status
        public override async Task OnConnectedAsync()
        {
            string userIdValue = Context.User.FindFirstValue(IdentityClaims.UserId);

            bool canParse = long.TryParse(userIdValue, out long userId);
            if (!canParse)
            {
                await Task.FromException(new ArgumentException("Invalid User ID"));
                return;
            }

            var result = await broker.CommandAsync(new UserConnectionOpenedCommand(userId, Context.ConnectionId));
            if (result.IsSuccess && result.Data.IsFirstConnection)
            {
                await Clients.All.SendAsync("UserIsOnline", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userIdValue = Context.User.FindFirstValue(IdentityClaims.UserId);

            bool canParse = long.TryParse(userIdValue, out long userId);
            if (!canParse)
            {
                await Task.FromException(new ArgumentException("Invalid User ID"));
                return;
            }

            var result = await broker.CommandAsync(new UserConnectionClosedCommand(userId, Context.ConnectionId));
            if (result.IsSuccess && result.Data.IsLastConnection)
            {
                await Clients.All.SendAsync("UserIsOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
