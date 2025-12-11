using ChatApp.Infrastructure.Brokers;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Infrastructure.Hubs
{
    public class ChatHub(IBroker broker) : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.All.SendAsync("ReceivedMessage", userId, message);
        }
    }
}
