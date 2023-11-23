using System.Security.Claims;
using Voicecord.Domain.Response;
using Voicecord.Domain.ViewModels.Account;
using Voicecord.Domain.ViewModels.Group;

namespace Voicecord.Interfaces
{
    public interface IGroupService
    {
        Task<BaseResponse<bool>> CreateGroup(CreateGroupViewModel model, string CreatorName);

    }
}
