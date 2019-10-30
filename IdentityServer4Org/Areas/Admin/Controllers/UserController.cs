using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.Email
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    //if (result.Succeeded)
                    //{
                    //    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    //    var comfirmationLink = Url.Action("ConfirmEmailAddress", "Home",
                    //        new { token = token, email = user.Email }, Request.Scheme);

                    //    System.IO.File.WriteAllText(@"C:\temp\comfirmation_link.txt", comfirmationLink);
                    //}
                }

                return RedirectToAction("Index", "Home", new { area = "Admin" } );
            }
            return View();
        }
    }
}
