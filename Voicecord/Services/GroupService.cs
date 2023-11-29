using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;
using Voicecord.Data.Repositories;
using Voicecord.Domain.Enum;
using Voicecord.Domain.Response;
using Voicecord.Domain.ViewModels.Account;
using Voicecord.Domain.ViewModels.Group;
using Voicecord.Helpers;
using Voicecord.Interfaces;
using Voicecord.Models;
using Voicecord.Service.Implementations;


namespace Voicecord.Services
{
    public class GroupService : IGroupService
    {
        private readonly IBaseRepository<UserGroup> groupRepository;
        private readonly IBaseRepository<ApplicationUser> userRepository;
        
        
        private readonly ILogger<AccountService> logger;
        public GroupService(IBaseRepository<UserGroup> groupRepository, IBaseRepository<ApplicationUser> userRepository,
            ILogger<AccountService> logger)
        {
            this.userRepository = userRepository;
            this.groupRepository = groupRepository;
            this.logger = logger;
        }

        public async Task<BaseResponse<bool>> AddToGroup(string groupLink, string userName)
        {
            var group=await groupRepository.GetAll().Include(x=>x.Users).FirstOrDefaultAsync(x=>x.LinkImageGroup==groupLink);
            if (group==null)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Сервера с таким url не существует",
                };
            }
            var user = userRepository.GetAll().FirstOrDefaultAsync(x => x.UserName == userName).Result;
            group.Users.Add(user);
            await groupRepository.Update(group);
            return new BaseResponse<bool>()
            {
                Data = true,
                Description = "Пользователь добавился",
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<bool>> CreateGroup(CreateGroupViewModel model, string creatorName)
        {
            try
            {        
                var group = await groupRepository.GetAll().FirstOrDefaultAsync(x => x.LinkImageGroup == model.GroupLink);
                if (group != null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Сервер с такой ссылкой уже есть",
                    };
                }

                var user = userRepository.GetAll().FirstOrDefaultAsync(x => x.UserName == creatorName).Result;
                group = new UserGroup()
                {
                    Name = model.NameGroup,
                    Chats = new List<Chat>(),
                    Voices = new List<VoiceChat>(),
                    LinkImageGroup = model.GroupLink,
                    Users = new List<ApplicationUser>() { user }
                };
                await groupRepository.Create(group);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Сервер добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[CreateGroup]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<List<UserGroup>> GetGroup(string UserName)
        {
            var groups = await groupRepository
                .GetAll()
                .Where(x => x.Users
                .Select(x=>x.UserName)
                .Contains(UserName)).ToListAsync();
            return groups;
        }
    }
}
