using Microsoft.AspNetCore.SignalR;

namespace Voicecord.Hubs
{
    public class HubRtc : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
