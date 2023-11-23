using Humanizer;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Voicecord.Hubs
{
    public class HubRtc : Hub
    {
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
