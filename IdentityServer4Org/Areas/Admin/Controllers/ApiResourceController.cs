using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ApiResourceController : Controller
    {
        private readonly IConfigurationDbContext configurationService;

        public ApiResourceController(IConfigurationDbContext configurationService)
        {
            this.configurationService = configurationService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ApiResourceInputModel model)
        {
            if (ModelState.IsValid)
            {
                var scopes = model.Scopes.Split(',');
                var apiScopes = scopes.Select(s => new ApiScope { Name = s }).ToList();

                await configurationService.ApiResources.AddAsync(new ApiResource
                {
                    Name = model.Name,
                    DisplayName = model.DisplayName,
                    Description = model.Description,
                    Scopes = apiScopes
                });

                await configurationService.SaveChangesAsync();

                return View("Success");
            }
            return View();
        }
    }
}
