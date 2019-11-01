using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
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

            return View(model);
        }
    }
}