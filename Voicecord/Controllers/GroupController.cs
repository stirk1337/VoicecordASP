using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voicecord.Domain.ViewModels.Account;
using Voicecord.Interfaces;
using Voicecord.Domain.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;

namespace Voicecord.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public IActionResult CreateGroup() => View();

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await groupService.CreateGroup(model, User.Identity.Name);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(response.Data));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", response.Description);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GetGroups()
        {
            return View(groupService.GetGroups(User.Identity.Name).Result);
        }

        [HttpGet]
        public async Task<IActionResult> AddToGroup(AddToGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await groupService.AddToGroup(model.GroupLink, User.Identity.Name);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetGroup", "Group");
                }
                ModelState.AddModelError("", response.Description);
            }
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetGroup(int id)
        {
            var group = await groupService.GetGroup(id);
            return View(group);
        }

    }
}
