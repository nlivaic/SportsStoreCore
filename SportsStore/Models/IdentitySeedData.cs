using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models {
    public class IdentitySeedData {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        
        public static async void EnsurePopulated(IApplicationBuilder app) {
            UserManager<IdentityUser> userManager = app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null) {
                user = new IdentityUser(adminUser);
                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}