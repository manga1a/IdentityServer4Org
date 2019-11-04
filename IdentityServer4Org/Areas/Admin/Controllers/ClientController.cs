using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4Org.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer4Org.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ClientController : Controller
    {
        private readonly IConfigurationDbContext configurationService;

        public ClientController(IConfigurationDbContext configurationService)
        {
            this.configurationService = configurationService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(JavaScriptClientInputModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                    ClientId = model.ClientId,
                    ClientName = model.ClientName,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = { model.RedirectUri },
                    PostLogoutRedirectUris = { model.PostLogoutRedirectUri },
                    AllowedCorsOrigins = { model.AllowedCorsOrigin },
                    AllowedScopes = model.AllowedScopes.Split(','),
                    RequireConsent = model.RequiredConsent
                };

                await configurationService.Clients.AddAsync(client.ToEntity());

                await configurationService.SaveChangesAsync();

                return View("Success");
            }
            return View();
        }
    }
}