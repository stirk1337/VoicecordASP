using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;
using Voicecord.Controllers;
using Voicecord.Data;
using Voicecord.Data.Repositories;
using Voicecord.Interfaces;
using Voicecord.Models;
using Voicecord.Service.Implementations;
using Voicecord.Services;
using Voicecord.Tests.Helpers;
using Voicecord.ViewModels.Group;

namespace Voicecord_Should.Controllers
{
    internal class GroupController_Should
    {
        GroupController controller;
        private readonly ILogger<AccountService> logger;
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
            userRepository= new UserRepository(dbContextMock.Object);
            groupService=new GroupService(groupRepository, userRepository, logger);
        }

        [SetUp]
        public void Setup()
        {
            InitializeService();
            controller = new GroupController(groupService);
        }
        [Test]
        public void GetBaseGroup()
        {
            groupService.GetGroups("user1").Result.Should().HaveCount(2);
            groupService.GetGroups("user4").Result.Should().HaveCount(1);
        }
        [Test]
        public void GetAllGroups()
        {
            groupService.GetAllGroups().Result.Should().HaveCount(2);
        }

    }
}
