using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
