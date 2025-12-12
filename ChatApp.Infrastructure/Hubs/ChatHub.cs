using ChatApp.Infrastructure.Brokers;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Infrastructure.Hubs
{
    public class ChatHub(IBroker broker) : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
