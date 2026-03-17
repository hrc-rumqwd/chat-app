using ChatApp.Application.Chat.Commands;
using ChatApp.Application.Contracts.Brokers;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Web.Hubs
{
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
    }
}
