using System.Security.Claims;
using Voicecord.ViewModels.Account;
using Voicecord.Models;
using Voicecord.Response;
using Voicecord.ViewModels.Group;

namespace Voicecord.Interfaces
{
    public interface IGroupService
    {
        Task<BaseResponse<bool>> CreateGroup(CreateGroupViewModel model, string CreatorName);
        Task<BaseResponse<bool>> CreateTextChat(CreateTextChatViewModel model, string CreatorName);
        Task<BaseResponse<bool>> CreateVoiceChat(CreateTextChatViewModel model, string CreatorName);
        Task<BaseResponse<bool>> AddToGroup(string groupLink, string userName);
        Task<List<UserGroup>> GetGroups(string UserName);
        Task<List<UserGroup>> GetAllGroups();
        Task<UserGroup> GetGroup(int groupId);
        Task AddMessageToDatabase(string linkGroup, string message, string creatorMessage, int chatId);


    }
}
