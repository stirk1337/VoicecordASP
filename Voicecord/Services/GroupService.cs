using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;
using Voicecord.Data.Repositories;
using Voicecord.ViewModels.Account;
using Voicecord.Helpers;
using Voicecord.Interfaces;
using Voicecord.Models;
using Voicecord.Response;
using Voicecord.Service.Implementations;
using Voicecord.ViewModels.Group;

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
        public async Task<BaseResponse<bool>> CreateVoiceChat(CreateTextChatViewModel model, string CreatorName)
        {
            try
            {
                var group = await groupRepository.GetAll().Include(x => x.Voices).FirstOrDefaultAsync(x => x.Id == model.GroupLink);
                if (group == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Неверный сервер",
                    };
                }
                var user = userRepository.GetAll().FirstOrDefaultAsync(x => x.UserName == CreatorName).Result;

                group.Voices.Add(new VoiceChat() { Name = model.NameGroup });

                await groupRepository.Update(group);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Голосовой чат добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[CreateVoiceChat]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> CreateTextChat(CreateTextChatViewModel model, string CreatorName)
        {
            try
            {
                var group = await groupRepository.GetAll().Include(x => x.Chats).ThenInclude(x => x.Messages).FirstOrDefaultAsync(x => x.Id == model.GroupLink);
                if (group == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Неверный сервер",
                    };
                }
                var user = userRepository.GetAll().FirstOrDefaultAsync(x => x.UserName == CreatorName).Result;

                group.Chats.Add(new Chat() { Name = model.NameGroup, Messages = new List<Message>() { new Message() { Date = DateTime.Now, Owner = user, TextMessage = "Создал новый чат" } } });

                await groupRepository.Update(group);
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Текстовый чат добавился",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[CreateTextChat]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> AddToGroup(string groupLink, string userName)
        {
            var group = await groupRepository.GetAll().Include(x => x.Users).FirstOrDefaultAsync(x => x.LinkImageGroup == groupLink);
            if (group == null)
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

                await AddGroupToDatabase(model, creatorName);
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
        public async Task AddMessageToDatabase(string linkGroup, string message, string creatorMessage, int chatId)
        {
            var group = await groupRepository.GetAll().Include(x => x.Chats).ThenInclude(x => x.Messages).Include(x => x.Users).Where(x => x.LinkImageGroup == linkGroup).FirstOrDefaultAsync();
            var user = userRepository.GetAll().Where(x => x.UserName == creatorMessage).FirstOrDefaultAsync().Result;
            if (user != null)
            {
                group.Chats.First(x => x.Id == chatId).Messages.Add(new Message() { Date = DateTime.Now, Owner = user, TextMessage = message });
            }
            await groupRepository.Update(group);
        }


        private async Task AddGroupToDatabase(CreateGroupViewModel model, string creatorName)
        {
            UserGroup group;
            var user = userRepository.GetAll().FirstOrDefaultAsync(x => x.UserName == creatorName).Result;
            group = new UserGroup()
            {
                Name = model.NameGroup,
                Chats = new List<Chat>() { new Chat() { Messages = new List<Message>(), Name = "first chat" } },
                Voices = new List<VoiceChat>() { new VoiceChat(), new VoiceChat() },
                LinkImageGroup = model.GroupLink,
                Users = new List<ApplicationUser>() { user }
            };
            await groupRepository.Create(group);
        }

        public async Task<UserGroup> GetGroup(int groupId)
        {
            var group = await groupRepository.GetAll()
                .Include(x => x.Users)
                .Include(x => x.Voices)
                .Include(x => x.Chats).ThenInclude(x => x.Messages)
                .Where(x => x.Id == groupId)
                .FirstAsync();
            return group;
        }

        public async Task<List<UserGroup>> GetGroups(string UserName)
        {
            var groups = await groupRepository
                .GetAll()
                .Where(x => x.Users
                .Select(x => x.UserName)
                .Contains(UserName)).ToListAsync();
            return groups;
        }


    }
}
