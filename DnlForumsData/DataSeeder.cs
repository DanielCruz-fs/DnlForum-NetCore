using DnlForumsData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsData
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext context;

        public DataSeeder(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(this.context);
            var userStore = new UserStore<ApplicationUser>(this.context);

            var user = new ApplicationUser()
            {
                UserName = "ForumAdmin",
                NormalizedUserName = "forumadmin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hasher.HashPassword(user, "admin");
            user.PasswordHash = hashedPassword;

            var hasAdminRole = this.context.Roles.Any(roles => roles.Name == "Admin");
            if (!hasAdminRole)
            {
                roleStore.CreateAsync(new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                });

            }

            var hasSuperUser = this.context.Users.Any(u => u.UserName == user.UserName);
            if (!hasSuperUser)
            {
                userStore.CreateAsync(user);
                userStore.AddToRoleAsync(user, "Admin");
            }

            this.context.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
