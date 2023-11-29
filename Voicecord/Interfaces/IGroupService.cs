using System.Security.Claims;
using Voicecord.Domain.Response;
using Voicecord.Domain.ViewModels.Account;
using Voicecord.Domain.ViewModels.Group;
using Voicecord.Models;

namespace Voicecord.Interfaces
{
    public interface IGroupService
    {
        Task<BaseResponse<bool>> CreateGroup(CreateGroupViewModel model, string CreatorName);
        Task<BaseResponse<bool>> AddToGroup(string groupLink, string userName);
        Task<List<UserGroup>> GetGroup(string UserName);


    }
}
