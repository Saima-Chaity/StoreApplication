using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StoreApplication.Data;
using StoreApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Repositories
{
    public class UserRoleRepo
    {
        IServiceProvider serviceProvider;

        public UserRoleRepo(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        // Assign a role to a user.
        public async Task<bool> AddUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.AddToRoleAsync(user, roleName);
            }
            return true;
        }

        // Get all roles of a specific user.
        public async Task<IEnumerable<RoleVM>> GetUserRoles(string email)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            var roles = await UserManager.GetRolesAsync(user);
            List<RoleVM> roleVMObjects = new List<RoleVM>();
            foreach (var item in roles)
            {
                roleVMObjects.Add(new RoleVM() { Id = item, RoleName = item });
            }
            return roleVMObjects;
        }
    }


}
