using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voicecord.Controllers;

namespace Voicecord_Should.Controllers
{
    internal class HomeController_Should
    {
        HomeController controller;
        private readonly ILogger<HomeController> _logger;
        [SetUp]
        public void Setup()
        {
            controller= new HomeController(_logger);
        }
        [Test]
        public void ShowViewIndex()
        {
            controller.Index().Should().As<ViewResult>();
        }
        [Test]
        public void ShowViewGetUserGroup()
        {
            controller.GetUserGroup().Should().As<ViewResult>();
        }
        [Test]
        public void ShowViewCreateGroup()
        {
            controller.CreateUserGroup().Should().As<ViewResult>();
        }
    }
}
