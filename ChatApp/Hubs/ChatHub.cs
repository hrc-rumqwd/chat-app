using ChatApp.Application.Contracts.Brokers;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Web.Hubs
{
    public class ChatHub(
        IBroker broker
    ) : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
