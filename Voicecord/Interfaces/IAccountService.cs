using System.Security.Claims;
using System.Threading.Tasks;
using Voicecord.Response;
using Voicecord.ViewModels.Account;

namespace Voicecord.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}
