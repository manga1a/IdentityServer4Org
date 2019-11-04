using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4Org.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Org.Data
{
    public static class SeedData
    {
        private static readonly string USER_NAME = "admin";
        private static readonly string ROLE_NAME = "Administrator";
        internal static async Task Initialize(IServiceProvider serviceProvider)
        {
            ILogger logger = Log.Logger;
            IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();

            // seed admin user
            var adminPassword = config["SeedAdminPassword"];
            var adminEmail = config["SeedAdminEmail"];

            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                await CreateAdminRole(serviceProvider, logger);
                await CreateAdminUser(serviceProvider, adminPassword, adminEmail, logger);
            }

            // seed identity resources
            using(var context = serviceProvider.GetRequiredService<ConfigurationDbContext>())
            {
                TrySeedIdentityResources(context);
            }
        }

        private static void TrySeedIdentityResources(ConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }

        private static IEnumerable<IdentityServer4.Models.IdentityResource> GetIdentityResources()
        {
            return new IdentityServer4.Models.IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        private static async Task CreateAdminRole(IServiceProvider serviceProvider, ILogger logger)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("RoleManager null");
            }

            if(await roleManager.RoleExistsAsync(ROLE_NAME)) { return;  }

            logger.Information("Attempting to create role {role}", ROLE_NAME);
            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(ROLE_NAME));

            if (result.Succeeded)
            {
                logger.Information("Successfully created new role {role}", ROLE_NAME);
            }
            else
            {
                throw new Exception("Failed to create new role");
            }
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider, string adminPassword, string adminEmail, ILogger logger)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(USER_NAME);

            if (user != null) { return; }

            logger.Information("Attempting to create new user {user}", USER_NAME);

            user = new IdentityUser
            {
                UserName = USER_NAME,
                Email = adminEmail
            };

            IdentityResult result = await userManager.CreateAsync(user, adminPassword);

            if (result.Succeeded)
            {
                logger.Information("{user} user created successfully. Attempting to assign role", USER_NAME);
            }
            else
            {
                throw new Exception("Failed to seed admin user. Make sure the password is strong enough.");
            }

            result = await userManager.AddToRoleAsync(user, ROLE_NAME);

            if (result.Succeeded)
            {
                logger.Information("{user} user added to {role} role successfully.", USER_NAME, ROLE_NAME);
            }
            else
            {
                logger.Error("Failed to add {user} user to {role} role.", USER_NAME, ROLE_NAME);
                foreach (var error in result.Errors)
                {
                    logger.Error("[{code}] {description}", error.Code, error.Description);
                }
            }
        }
    }
}
