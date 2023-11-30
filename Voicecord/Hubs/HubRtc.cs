using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Voicecord.Hubs
{
    public class HubRtc : Hub
    {
        private static readonly ConcurrentDictionary<string, string> connectedUsers = new ConcurrentDictionary<string, string>();

        public async Task NewConnection(string user)
        {
            connectedUsers.TryAdd(user, Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var item = connectedUsers.First(kvp => kvp.Value == Context.ConnectionId);
                connectedUsers.TryRemove(item);
                await Clients.All.SendAsync("UserDisconnected", item.Key);
            }
            catch { }
        }

        public async Task GetConnectedUsers()
        {
            Console.WriteLine(connectedUsers.Count);
            await Clients.Caller.SendAsync("GetConnectedUsers", connectedUsers.Keys);
        }
    
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendOfferCandidates(string user, string user_to, string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            await Clients.Client(connectedUsers[user_to]).SendAsync("ReceiveOfferCandidates", user, candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }

        public async Task SendAnswerCandidates(string user, string user_to, string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            await Clients.Client(connectedUsers[user_to]).SendAsync("ReceiveAnswerCandidates", user, candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }

        public async Task SendOffer(string user, string user_to, string sdp, string type)
        {
            await Clients.Client(connectedUsers[user_to]).SendAsync("ReceiveOffer", user, sdp, type);
        }

        public async Task SendAnswer(string user, string user_to, string sdp, string type)
        {
            await Clients.Client(connectedUsers[user_to]).SendAsync("ReceiveAnswer", user, sdp, type);
        }
    }
}
