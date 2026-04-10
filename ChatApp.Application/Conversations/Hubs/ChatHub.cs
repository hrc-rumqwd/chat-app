using ChatApp.Application.Chat.Commands;
using ChatApp.Application.Contracts.Brokers;
using ChatApp.Application.Users.Commands;
using ChatApp.Application.Users.Queries;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Models.Commons;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Application.Conversations.Hubs
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ChatHub(
            IBroker broker
        ) : Hub
    {
        public async Task SendMessage(string message, long userId, long conversationId)
        {
            Result<SendMessageCommandResult> sendMessageResult = await broker.CommandAsync(new SendMessageCommand(message, userId, conversationId));

            if (!sendMessageResult.IsSuccess)
            {
                await Clients.Caller.SendAsync("OnSendMessageError", sendMessageResult);
                return;
            }

            await Clients.Caller.SendAsync("ReceiveMessage", sendMessageResult);

            #region One-to-One chat
            // Searching for receiver's connection id to send realtime message
            if (!sendMessageResult.Data.IsGroup)
            {
                Result<string[]> connectionIds = await broker.QueryAsync(new GetUserConnectionsQuery(sendMessageResult.Data.OneToOneReceiver.Id));
                // Only send realtime message if receiver is online, otherwise just save message to database and wait for receiver to get it when they come online
                if (connectionIds.Data.Length > 0)
                {
                    await Clients.Clients(connectionIds.Data).SendAsync("ReceiveMessage", sendMessageResult);
                }
            }
            #endregion

            #region Group chat
            // Search for multi member's connection ids to send realtime message
            if (sendMessageResult.Data.IsGroup)
            {
                Result<Dictionary<long, string[]>> connectionIds = await broker.QueryAsync(new GetUsersConnectionsQuery(sendMessageResult.Data.Participants.Select(c => c.Id)));
                IEnumerable<string> receivedClients = connectionIds.Data
                    .Where(c => c.Value.Length > 0) // Only send realtime message to online users
                    .SelectMany(c => c.Value);

                await Clients.Clients(receivedClients).SendAsync("ReceiveMessage", sendMessageResult);
            }
            #endregion
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

            Result<UserConnectionOpenedCommandResult> result = await broker.CommandAsync(new UserConnectionOpenedCommand(userId, Context.ConnectionId));
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

            Result<UserConnectionClosedCommandResult> result = await broker.CommandAsync(new UserConnectionClosedCommand(userId, Context.ConnectionId));
            if (result.IsSuccess && result.Data.IsLastConnection)
            {
                await Clients.All.SendAsync("UserIsOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
