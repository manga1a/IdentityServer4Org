using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Services;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailSender emailSender;

        public UserController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
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

                    string temporaryPassword = GenerateRandomPassword(userManager.Options.Password);
                    var result = await userManager.CreateAsync(user, temporaryPassword);

                    if (result.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var comfirmationLink = Url.Action("ConfirmEmailAddress", "Account",
                            new { area = "", token, email = user.Email, reset = true }, Request.Scheme);

                        emailSender.Send(user.Email, "Complete registration", GenerateConfirmationMessage(comfirmationLink));
                    }
                }

                return View("Success");
            }
            return View();
        }

        private static string GenerateConfirmationMessage(string link)
        {
            string plainText = @$"Hi,

Please visit following link to complete registration process.

{link}

-- IdentityServer4Org Admin
";

            return plainText;
        }

        private static string GenerateRandomPassword(PasswordOptions opts)
        {
            //https://www.ryadel.com/en/c-sharp-random-password-generator-asp-net-core-mvc/

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
            };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
