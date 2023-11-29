using Humanizer;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voicecord.Domain.ViewModels.Account;
using Voicecord.Interfaces;
using Voicecord.Domain.ViewModels.Group;
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

        public async Task GetConnectedUsers()
        {
            Console.WriteLine(connectedUsers.Count);
            await Clients.All.SendAsync("GetConnectedUsers", connectedUsers.Keys);
        }
    
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendOfferCandidates(string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            await Clients.Others.SendAsync("ReceiveOfferCandidates", candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }
        public async Task SendAnswerCandidates(string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            await Clients.Others.SendAsync("ReceiveAnswerCandidates", candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }

        public async Task SendOffer(string sdp, string type)
        {
            await Clients.Others.SendAsync("ReceiveOffer", sdp, type);
        }

        public async Task SendAnswer(string sdp, string type)
        {
            await Clients.Others.SendAsync("ReceiveAnswer", sdp, type);
        }
    }
}
