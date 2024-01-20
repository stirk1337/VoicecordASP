using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Voicecord.Interfaces;

namespace Voicecord.Hubs
{
    public class HubRtc : Hub
    {
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> groupsConnectedUsers = new ConcurrentDictionary<string, Dictionary<string, string>>();
        private static readonly ConcurrentDictionary<string, string> userGroups = new ConcurrentDictionary<string, string>();
        private readonly IGroupService groupService;
        private readonly ILogger<HubRtc> logger;

        public HubRtc(IGroupService groupService, ILogger<HubRtc> logger)
        {
            this.groupService = groupService;
            this.logger = logger;
        }

        public async Task NewConnection(string group)
        {
            if (!groupsConnectedUsers.ContainsKey(group))
            {
                groupsConnectedUsers.TryAdd(group, new Dictionary<string, string>());
            }
            groupsConnectedUsers[group][Context.User.Identity.Name] = Context.ConnectionId;
            userGroups.TryAdd(Context.User.Identity.Name, group);
            userGroups[Context.User.Identity.Name] = group;
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await SendUsersVoiceChat(groupsConnectedUsers);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User.Identity.Name;
            try
            {
                var group = userGroups[username];
                var item = groupsConnectedUsers[group].FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
                groupsConnectedUsers[group].Remove(item.Key);
                await Clients.OthersInGroup(group).SendAsync("UserDisconnected", item.Key, group);
                await SendUsersVoiceChat(groupsConnectedUsers);
            }
            catch
            {
                logger.LogInformation("Exception in on disconnected");
            }
        }


        public async Task VoiceDisconnectButton()
        {
            var username = Context.User.Identity.Name;
            try
            {
                var group = userGroups[username];
                var item = groupsConnectedUsers[group].FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
                groupsConnectedUsers[group].Remove(item.Key);
                await Clients.OthersInGroup(group).SendAsync("UserDisconnected", item.Key, group);
                await SendUsersVoiceChat(groupsConnectedUsers);
            }
            catch
            {
                logger.LogInformation("Exception in on disconnected");
            }
        }

        public async Task GetConnectedUsers()
        {
            var username = Context.User.Identity.Name;
            var group = userGroups[username];
            logger.LogInformation("Connected users: " + groupsConnectedUsers[group].Count.ToString());
            await Clients.Caller.SendAsync("GetConnectedUsers", groupsConnectedUsers[group].Keys, group);
        }

        public async Task SendUsersVoiceChat(ConcurrentDictionary<string, Dictionary<string, string>> groupsConnectedUsers)
        {
            var username = Context.User.Identity.Name;
            var group = userGroups[username];
            await Clients.All.SendAsync("SendAllUsersVoiceChat", groupsConnectedUsers);
        }

        public async Task SendMessage(string message, string linkGroup, string? chatId, string disscusionId)
        {
            await groupService.AddMessageToDatabase(linkGroup, message, Context.User.Identity.Name, int.Parse(chatId));
            await Clients.Group(userGroups[Context.User.Identity.Name]).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, disscusionId, DateTime.Now.ToShortTimeString());
        }

        public async Task SendOfferCandidates(string user, string user_to, string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            var group = userGroups[user];
            await Clients.Client(groupsConnectedUsers[group][user_to]).SendAsync("ReceiveOfferCandidates", user, candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }

        public async Task SendAnswerCandidates(string user, string user_to, string candidate, int sdpMLineIndex, string sdpMid, string usernameFragment)
        {
            var group = userGroups[user];
            await Clients.Client(groupsConnectedUsers[group][user_to]).SendAsync("ReceiveAnswerCandidates", user, candidate, sdpMLineIndex, sdpMid, usernameFragment);
        }

        public async Task SendOffer(string user, string user_to, string sdp, string type)
        {
            var group = userGroups[user];
            await Clients.Client(groupsConnectedUsers[group][user_to]).SendAsync("ReceiveOffer", user, group, sdp, type);
        }

        public async Task SendAnswer(string user, string user_to, string sdp, string type)
        {
            var group = userGroups[user];
            await Clients.Client(groupsConnectedUsers[group][user_to]).SendAsync("ReceiveAnswer", user, sdp, type);
        }
    }
}
