using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Voicecord.Controllers;
using Voicecord.Data;
using Voicecord.Data.Repositories;
using Voicecord.Hubs;
using Voicecord.Interfaces;
using Voicecord.Service.Implementations;
using Voicecord.Services;
using Voicecord.Tests.Helpers;

namespace Voicecord.Tests.SignalR
{
    internal class SignalR_Should
    {
        GroupController controller;
        private readonly ILogger<AccountService> accountLogger;
        private readonly ILogger<HubRtc> hubLogger;
        private GroupRepository groupRepository;
        private UserRepository userRepository;

        private IGroupService groupService;

        private DbContextOptions<ApplicationDbContext> options;


        private void InitializeService()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
            var dbContextMock = new Mock<ApplicationDbContext>(options);
            dbContextMock.Setup(x => x.Users).ReturnsDbSet(TestDataHelper.GetFakeUsersList());
            dbContextMock.Setup(x => x.Groups).ReturnsDbSet(TestDataHelper.GetFakeGroupList());
            groupRepository = new GroupRepository(dbContextMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
            groupService = new GroupService(groupRepository, userRepository, accountLogger);
           
        }

        [SetUp]
        public void Setup()
        {
            InitializeService();
        }


        [Test]
        public async Task SignalR_OnConnect_ShouldReturn3Messages()
        { 
        }
    }
}
