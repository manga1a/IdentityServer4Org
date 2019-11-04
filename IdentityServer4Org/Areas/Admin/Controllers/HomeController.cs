using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfigurationDbContext configurationService;

        public HomeController(UserManager<IdentityUser> userManager, IConfigurationDbContext configurationService)
        {
            this.userManager = userManager;
            this.configurationService = configurationService;
        }

        public IActionResult Index()
        {
            var model = new AdminHomeViewModel();

            foreach(var identityUser in userManager.Users)
            {
                model.Users.Add(new UserViewModel
                {
                    UserId = identityUser.Id,
                    UserName = identityUser.UserName,
                    Email = identityUser.Email
                });
            }

            foreach(var apiResource in configurationService.ApiResources)
            {
                model.ApiResources.Add(new ApiResourceViewModel
                {
                    Id = apiResource.Id,
                    Name = apiResource.Name,
                    DisplayName = apiResource.DisplayName,
                    Description = apiResource.Description
                });
            }

            foreach(var client in configurationService.Clients.Where(c => c.Enabled == true))
            {
                model.Clients.Add(new ClientViewModel
                {
                    Id = client.Id,
                    ClientId = client.ClientId,
                    Name = client.ClientName,
                    Description = client.Description
                });
            }

            return View(model);
        }
    }
}