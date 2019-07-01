using DnlForumsData;
using DnlForumsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsService
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return this.context.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return this.GetAll().FirstOrDefault(user => user.Id == id);
        }

        public Task IncrementRating(string id, Type type)
        {
            throw new NotImplementedException();
        }

        public async Task SetProfileImage(string id, string imageUrl)
        {
            var user = this.GetById(id);
            user.ProfileUrl = imageUrl;
            this.context.Update(user);
            await this.context.SaveChangesAsync();
        }
    }
}
